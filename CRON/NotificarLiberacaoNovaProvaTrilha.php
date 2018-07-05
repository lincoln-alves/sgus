<?php

function cron(){
    $content = array();
    $config = array();

    $config['method'] = 'NotificarLiberacaoNovaProvaTrilha';
	$config['args'] = array();
    $content['lista'] = _sebraeuc_consume_service($config['method'], $config['args']);

    header('Content-Type: application/json');

    echo json_encode($content['lista']);
}


function _sebraeuc_consume_service($metodo, $args){
    try {
        // Create the SoapClient instance
        //$url        = (!$server)?variable_get('sebraeuc_default_server'):$server;
		$url 		= 'http://homolog.ws_sgus20.icomunicacao.com.br/SgusWebService.asmx?WSDL';
        $client    	= new SoapClient($url, array("connection_timeout" => 250));
		ini_set('default_socket_timeout', 250);
        // Create the header

        $accountName    = '91697565204';
        $accountPassword = 'sebrae2016';

        $accountSession = session_id();
        $auth = array('Login' => $accountName, "Senha" => $accountPassword, "SessionID" => $accountSession );
        $header = new SoapHeader("http://tempuri.org/", "AuthenticationRequest", $auth, false);

        $client->__setSoapHeaders($header);

        // Call wsdl function
        if($metodo){

            $result = $client->$metodo($args);

            $metodoResult = $metodo . 'Result';

            if(property_exists($result, $metodoResult))
            {
                if(!is_object($result->$metodoResult) || (!property_exists($result->$metodoResult, 'Erro') || $result->$metodoResult->Erro == 0 )){
                    return array(
                        'msg' => isset($result->$metodoResult->Mensagem) ? $result->$metodoResult->Mensagem : "",
                        'status' => isset($result->$metodoResult->Erro) ? ($result->$metodoResult->Erro == 0 ? 'success' : 'error') : "",
                        'code' => isset($result->$metodoResult->Erro) ? $result->$metodoResult->Erro : ""
                    );
                }
                else{
                    return $result;
                }
            }

            return $result;
        }
    } catch (Exception $e) {
        $error_msg = $e->getMessage();

        return array(
            'msg' => $error_msg,
            'status' => 'error',
            'code' => 1
        );
    }
}