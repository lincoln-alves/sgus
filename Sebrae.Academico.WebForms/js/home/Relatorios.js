function executarRelatorioConcluintesPorRegiao(texto) {

    if (texto) {
        var json = JSON.parse(texto);
    }

    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Regiao');
    data.addColumn('number', 'Concluintes');

    for (var i = 0; i < json.length; i++) {
        data.addRow([json[i].Regiao, json[i].Concluintes]);
    }

    var options = { title: '% DE CONCLUINTES POR REGIÃO', legend: { position: 'bottom' } };
    var chart = new google.visualization.BarChart(document.getElementById('grafico-concluintes-por-regiao'));
    chart.draw(data, options);

}

function executarRelatorioConcluintesPorSolucaoEducacional(texto) {
    if (texto) {
        var json = JSON.parse(texto);
    }

    var data = new google.visualization.DataTable();
    data.addColumn('string', 'SolucaoEducacional');
    data.addColumn('number', 'Concluintes');

    for (var i = 0; i < json.length; i++) {
        data.addRow([json[i].SolucaoEducacional, json[i].Concluintes]);
    }

    var options = { title: 'SOLUÇÕES EDUCACIONAIS COM MAIORES NÚMEROS DE CONCLUINTES', legend: { position: 'bottom' } };
    var chart = new google.visualization.BarChart(document.getElementById('graf-concluintes'));
    chart.draw(data, options);
}

function executarRelatorioFaixaEtaria(texto) {
    if (texto) {
        var json = JSON.parse(texto);
    }

    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Nome');
    data.addColumn('number', 'Quantidade');

    for (var i = 0; i < json.length; i++) {
        data.addRow([json[i].Nome, json[i].Quantidade]);
    }

    var options = { title: '% DE FAIXA ETÁRIA', legend: { position: 'bottom' } };
    var chart = new google.visualization.BarChart(document.getElementById('grafico-faixa-etaria'));
    chart.draw(data, options);
}

function executarRelatorioPerfilDoPublicoAtendido(texto) {
    if (texto) {
        var json = JSON.parse(texto);
    }

    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Perfil');
    data.addColumn('number', 'Quantidade');

    for (var i = 0; i < json.length; i++) {
        data.addRow([json[i].Perfil, json[i].Quantidade]);
    }

    var options = { title: '% DE LIMITES POR SEBRAE-DF', legend: { position: 'bottom' } };
    var chart = new google.visualization.BarChart(document.getElementById('grafico-perfil-do-publico-concluinte'));
    chart.draw(data, options);
}

function executarRelatorioLimitesPorSebraeDF(texto) {
    if (texto) {
        var json = JSON.parse(texto);
    }

    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Nome');
    data.addColumn('number', 'Quantidade');

    for (var i = 0; i < json.length; i++) {
        data.addRow([json[i].Nome, json[i].Quantidade]);
    }

    var options = { title: '% DE LIMITES POR SEBRAE-DF', legend: { position: 'bottom' } };
    var chart = new google.visualization.BarChart(document.getElementById('grafico-limites-por-uf'));
    chart.draw(data, options);
}

function concluintesPorRegiao() {
    if (event.keyCode == 13) {
        $("#ContentPlaceHolder1_txtFiltrarPorAnoConcluintePorRegiao").attr("disabled", "disabled");

        __doPostBack('ctl00$ContentPlaceHolder1$lnkConcluintesPorRegiao', '');

        var contador = 0;

        var evento = setInterval(function () {
            contador++;

            var texto = $("#ContentPlaceHolder1_hdConcluintesPorRegiao").val();

            if (texto) {
                executarRelatorioConcluintesPorRegiao(texto);
                clearInterval(evento);
                $("#ContentPlaceHolder1_txtFiltrarPorAnoConcluintePorRegiao").removeAttr("disabled", "disabled");
            }

            if (contador > 5) {
                alert("Sem resultados para o ano selecionado.")
                $("#ContentPlaceHolder1_txtFiltrarPorAnoConcluintePorRegiao").removeAttr("disabled", "disabled");
                clearInterval(evento);
                return;
            }

        }, 1000);
    }
}

function concluintesPorSolucaoEducacional() {
    if (event.keyCode == 13) {
        $("#ContentPlaceHolder1_txtConcluintesPorSolucaoEducacional").attr("disabled", "disabled");

        __doPostBack('ctl00$ContentPlaceHolder1$lnkConcluintesPorSolucaoEducacional', '');

        var contador = 0;

        var evento = setInterval(function () {
            contador++;

            var texto = $("#ContentPlaceHolder1_hdConcluintesPorSolucaoEducacional").val();

            if (texto) {
                executarRelatorioConcluintesPorSolucaoEducacional(texto);
                clearInterval(evento);
                $("#ContentPlaceHolder1_txtConcluintesPorSolucaoEducacional").removeAttr("disabled", "disabled");
            }

            if (contador > 5) {
                alert("Sem resultados para o ano selecionado.")
                $("#ContentPlaceHolder1_txtConcluintesPorSolucaoEducacional").removeAttr("disabled", "disabled");
                clearInterval(evento);
                return;
            }

        }, 1000);
    }
}

function faixaEtaria() {
    if (event.keyCode == 13) {
        $("#ContentPlaceHolder1_txtFaixaEtaria").attr("disabled", "disabled");

        __doPostBack('ctl00$ContentPlaceHolder1$lnkFaixaEtaria', '');

        var contador = 0;

        var evento = setInterval(function () {
            contador++;

            var texto = $("#ContentPlaceHolder1_hdFaixaEtaria").val();

            if (texto) {
                executarRelatorioFaixaEtaria(texto);
                $("#ContentPlaceHolder1_txtFaixaEtaria").removeAttr("disabled", "disabled");
                clearInterval(evento);
            }

            if (contador > 5) {
                alert("Sem resultados para o ano selecionado.")
                $("#ContentPlaceHolder1_txtFaixaEtaria").removeAttr("disabled", "disabled");
                clearInterval(evento);
                return;
            }

        }, 1000);
    }
}

function perfilDoPublicoAtendido() {
    if (event.keyCode == 13) {
        $("#ContentPlaceHolder1_txtPerfilDoPublicoAtendimento").attr("disabled", "disabled");

        __doPostBack('ctl00$ContentPlaceHolder1$lnkPerfilDoPublicoAtendimento', '');

        var contador = 0;

        var evento = setInterval(function() {
            contador++;

            var texto = $("#ContentPlaceHolder1_hdPerfilDoPublicoAtendimento").val();

            if (texto) {
                executarRelatorioPerfilDoPublicoAtendido(texto);
                $("#ContentPlaceHolder1_txtPerfilDoPublicoAtendimento").removeAttr("disabled", "disabled");
                clearInterval(evento);
            }

            if (contador > 5) {
                alert("Sem resultados para o ano selecionado.");
                $("#ContentPlaceHolder1_txtPerfilDoPublicoAtendimento").removeAttr("disabled", "disabled");
                clearInterval(evento);
                return;
            }

        }, 1000);
    }
}

function limitesPorSebraeDF() {
    if (event.keyCode == 13) {
        $("#ContentPlaceHolder1_txtLimitesPorSebraeDF").attr("disabled", "disabled");

        __doPostBack('ctl00$ContentPlaceHolder1$lnkLimitesPorSebraeDF', '');

        var contador = 0;

        var evento = setInterval(function () {
            contador++;

            var texto = $("#ContentPlaceHolder1_hdLimitesPorSebraeDF").val();

            if (texto) {
                executarRelatorioLimitesPorSebraeDF(texto);
                $("#ContentPlaceHolder1_txtLimitesPorSebraeDF").removeAttr("disabled", "disabled");
                clearInterval(evento);
            }

            if (contador > 5) {
                alert("Sem resultados para o ano selecionado.");
                $("#ContentPlaceHolder1_txtLimitesPorSebraeDF").removeAttr("disabled", "disabled");
                clearInterval(evento);
                return;
            }

        }, 1000);
    }
}