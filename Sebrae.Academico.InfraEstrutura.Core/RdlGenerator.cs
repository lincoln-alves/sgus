using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices.ReportProcessing.OnDemandReportObjectModel;
using Rdl;
using Report = Rdl.Report;

namespace Sebrae.Academico.InfraEstrutura.Core
{
    class RdlColumnHeader
    {
        public string GroupBy { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public double Width { get; set; }
        public bool Visible { get; set; }
    }
    class RdlGenerator
    {
        public IList<RdlColumnHeader> Fields { get; set; }
        public IList<string> TituloHeader { get; set; }

        public RdlGenerator()
        {
            TituloHeader = new List<string>();
            Fields = new List<RdlColumnHeader>();
        }

        private Report CreateReport()
        {
            var report = new Report
            {
                Items = new object[] {
                    CreateDataSources(),
                    CreateDataSets(),
                    CreateBody(),
                    "8.18301in",
                    CreatePage(),
                    LoadImages()
                },
                ItemsElementName = new[] {
                    ItemsChoiceType80.DataSources,
                    ItemsChoiceType80.DataSets,
                    ItemsChoiceType80.Body,
                    ItemsChoiceType80.Width,
                    ItemsChoiceType80.Page,
                    ItemsChoiceType80.EmbeddedImages,
                }
            };
            return report;
        }

        private ReportParametersType CreateParameters()
        {
            var list = new List<ReportParameterType>();

            if (Fields != null && Fields.Count > 0)
            {
                foreach (var item in Fields)
                {
                    list.Add(new ReportParameterType
                    {
                        Items = new object[] {
                            ReportParameterTypeDataType.String,
                            "ReportParameter1"
                        },
                        ItemsElementName = new[] {
                            ItemsChoiceType75.DataType,
                            ItemsChoiceType75.Prompt,
                        },
                        Name = item.Value + "Visible"
                    });
                }
            }

            return new ReportParametersType
            {
                ReportParameter = list.ToArray()
            };
        }

        private static EmbeddedImagesType LoadImages()
        {
            return new EmbeddedImagesType
            {
                EmbeddedImage = new[] {
                    new EmbeddedImageType {
                        Name = "Cabecalho",
                        Items = new object[] {
                            "image/gif",
                            @"R0lGODlh0AdVAPcAAFuYrKDG6yp/0jKE1J7F6zuJ1mKg3tjo92Kbo2+o4TSF1JfA6Yy654q55x530CN60R940Cp+zNfdjSl+0pO+6D2K1iyA0y6B0+HlqFaZ3LbT8D6L1zeG1UiR2TeFxoS15VSY2yB50KjL7Xyw5ESO2Bx2zyV70aSzD3OjjKe0C3akhHmu45yxJpGuQ3+ndVSUskyQuKGyGSF6zkuQu5yxKi6AykGM1xp1z4WqZqa0DUGKwh95z6i1CX6x5P7+//z9/id90v3+/0KN1/v8/mWi3/b6/VOX2/T5/Ya25u/1/EqS2Vyd3UeQ2PL3/Pf6/fj7/fn7/uvz+5W/6TiH1fH2/Ojx+ht2z9zq+KzN7ubw+tLk9pS/6enx+uPu+YO05dbm9vv9/uLu+e71+4+758vg9HCp4TmH1bTS76bJ7Im45sbc88Tb8+Xv+pC86Nfn92Cf3uny+sfd88/i9cXc83+y5DmI1t3q+L3X8bPR74e25k+V2kOO2Ii35vr8/nGp4eTv+b/Z8o6753uv45nB6tTl9mSh3rHQ7+Dt+bvW8aTI7PP4/UaQ2NDj9ZjB6XSr4l2e3aXJ7Hqv426n4c3h9JzD6lqb3Mbd8+Ds+Orz+06U2oi45rjU8G2n4Ofw+lia3FCV2sDZ8oGz5ZvD6sPb8rrV8fX5/bzW8aLH697r+L7Y8WGg3szg9LLQ76rM7cje9Hat4uz0+16e3c7h9aPH7NHj9drp953E6maj38Ha8lmb3Gek39Pk9t/s+CZ80ZrC6vD2/F+f3Wym4ICz5K/P7jCC1DGD1Nvp92ql4DCD1HOr4nKq4Vuc3aPI7KfK7bfU8O30+7DP7snf9JG86JG96KvM7Wum4EuT2a7O7kyT2lKX24K05eHt+bnV8FGW2k2U2lOY23et48rf9KvN7Wml4Has4sLa8oyrVWufl3Ws4nmkgCd8zJSuOpewNY2rTIa11sTNWHumfLK9JDqI0LvFPomrYs3V
cmGanICnclCSt/n57Wik35euNam2DZivM7G9Iv///yH/C1hNUCBEYXRhWE1QPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS41LWMwMjEgNzkuMTU0OTExLCAyMDEzLzEwLzI5LTExOjQ3OjE2ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSJ4bXAuZGlkOjlkNGRkYjc5LWRjMjgtNzQ0MC1hNDZkLWVkZWVhNTdhYTgwMCIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDo5NDVFNzM3N0EwRDUxMUUzQjQ3NzlEODAzRThFRUVBOSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDo5NDVFNzM3NkEwRDUxMUUzQjQ3NzlEODAzRThFRUVBOSIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6OWQ0ZGRiNzktZGMy
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"
                        },
                        ItemsElementName = new [] {
                            ItemsChoiceType78.MIMEType,
                            ItemsChoiceType78.ImageData,
                        }
                    },
                    new EmbeddedImageType {
                        Name = "Rodape",
                        Items = new object[] {
                            "image/gif",
                            @"R0lGODlh0AdVAIAAALG9Iv///yH/C1hNUCBEYXRhWE1QPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS41LWMwMjEgNzkuMTU0OTExLCAyMDEzLzEwLzI5LTExOjQ3OjE2ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdFJlZj0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlUmVmIyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjI2MTUyNUJCOUY1ODExRTM4MDU4RDYzNUI4NjVCRkRCIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjI2MTUyNUJDOUY1ODExRTM4MDU4RDYzNUI4NjVCRkRCIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MjYxNTI1Qjk5RjU4MTFFMzgwNThENjM1Qjg2NUJGREIiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MjYxNTI1QkE5RjU4MTFFMzgwNThENjM1Qjg2NUJGREIiLz4g
PC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz4B//79/Pv6+fj39vX08/Lx8O/u7ezr6uno5+bl5OPi4eDf3t3c29rZ2NfW1dTT0tHQz87NzMvKycjHxsXEw8LBwL++vby7urm4t7a1tLOysbCvrq2sq6qpqKempaSjoqGgn56dnJuamZiXlpWUk5KRkI+OjYyLiomIh4aFhIOCgYB/fn18e3p5eHd2dXRzcnFwb25tbGtqaWhnZmVkY2JhYF9eXVxbWllYV1ZVVFNSUVBPTk1MS0pJSEdGRURDQkFAPz49PDs6OTg3NjU0MzIxMC8uLSwrKikoJyYlJCMiISAfHh0cGxoZGBcWFRQTEhEQDw4NDAsKCQgHBgUEAwIBAAAh+QQAAAAAACwAAAAA0AdVAAAC/4SPqcvtD6OctNqLs968+w+G4kiW5omm6sq27gvH8kzX9o3n+s73/g8MCofEovGITCqXzKbzCY1Kp9Sq9YrNarfcrvcLDovH5LL5jE6r1+y2+w2Py+f0uv2Oz+v3/L7/DxgYGEBYaHiImKi4yNjo+AgZKTlJWWl5iZmpucnZ6fkJGio6SlpqeoqaqrrK2ur6ChsrO0tba3uLm6u7y9vr+wscLDxMXGx8jJysvMzc7PwMHS09TV1tfY2drb3N3e39DR4uPk5ebn6Onq6+zt7u/g4fLz9PX29/j5+vv8/f7/8PMKDAgQQLGjyIMKHChQwbOnwIMaLEiRQrWryIMaPGjf8cO3r8CDKkyJEkS5o8iTKlypUsW7p8CTOmzJk0a9q8iTOnzp08e/r8CTSo0KFEixo9ijSp0qVMmzp9CjWq1KlUq1q9ijWr1q1cu3r9Cjas2LFky5o9izat2rVs27p9Czeu3Ll069q9izev3r18+/r9Cziw4MGECxs+jDix4sWMGzt+DDmy5MmUK1u+jDmz5s2cO3v+DDq06NGk
S5s+jTq16tWsW7t+DTu27Nm0a9u+jTu37t28e/v+DTy48OHEixs/jjy58uXMmzt/Dj269OnUq1u/jj279u3cu3v/Dj68+PHky5s/jz69+vXs27t/Dz++/Pn069u/jz+//v38+/vu/w9ggAIOSGCBBh6IYIIKLshggw4+CGGEEk5IYYUWXohhhhpuyGGHHn4IYogijkhiiSaeiGKKKq7IYosuvghjjDLOSGONNt6IY4467shjjz7+CGSQQg5JZJFGHolkkkouyWSTTj4JZZRSTklllVZeiWWWWm7JZZdefglmmGKOSWaZZp6JZppqrslmm26+CWeccs5JZ5123olnnnruyWeffv4JaKCCDkpooYYeimiiii7KaKOOPgpppJJOSmmlll6Kaaaabsppp55+Cmqooo5Kaqmmnopqqqquymqrrr4Ka6yyzkprrbbeimuuulpYAAA7"
                        },
                        ItemsElementName = new [] {
                            ItemsChoiceType78.MIMEType,
                            ItemsChoiceType78.ImageData,
                        }
                    },
                }
            };
        }
        private PageType CreatePage()
        {
            return new PageType
            {
                Items = new object[] {
                    CreatePageHeader(),
                    CreatePageFooter(),
                    "29.7cm",
                    "21cm",
                    "2cm",
                    "2cm",
                    "2cm",
                    "0.13cm"
                },
                ItemsElementName = new[] {
                    ItemsChoiceType77.PageHeader,
                    ItemsChoiceType77.PageFooter,
                    ItemsChoiceType77.PageWidth,
                    ItemsChoiceType77.PageHeight,
                    ItemsChoiceType77.LeftMargin,
                    ItemsChoiceType77.RightMargin,
                    ItemsChoiceType77.TopMargin,
                    ItemsChoiceType77.BottomMargin,
                    ItemsChoiceType77.ColumnSpacing
                }
            };
        }

        #region Header
        public PageSectionType CreatePageHeader()
        {
            return new PageSectionType
            {
                Items = new object[] {
                    "2.40771cm",
                    true,
                    true,
                    CreateHeaderReportItems(),
                    CreateHeaderStyle()
                },
                ItemsElementName = new[] {
                    ItemsChoiceType76.Height,
                    ItemsChoiceType76.PrintOnFirstPage,
                    ItemsChoiceType76.PrintOnLastPage,
                    ItemsChoiceType76.ReportItems,
                    ItemsChoiceType76.Style,
                }
            };
        }

        private ReportItemsType CreateHeaderReportItems()
        {
            return new ReportItemsType
            {
                Items = new object[] {
                    CreateHeaderTextBox()
                }
            };
        }

        private TextboxType CreateHeaderTextBox()
        {
            return new TextboxType
            {
                Name = "textBox" + Guid.NewGuid().ToString().Split('-')[0],
                Items = new object[] {
                    CreateHeaderTextBoxParagraphs(),
                    "0.19532cm",
                    "4.99217cm",
                    "1.68479cm",
                    CreateHeaderTextBoxStyle()
                },
                ItemsElementName = new[] {
                    ItemsChoiceType14.Paragraphs,
                    ItemsChoiceType14.Top,
                    ItemsChoiceType14.Left,
                    ItemsChoiceType14.Height,
                    ItemsChoiceType14.Style
                }
            };
        }

        private static StyleType CreateHeaderTextBoxStyle()
        {
            return new StyleType
            {
                Items = new object[] {
                    "2pt",
                    "2pt",
                    "2pt",
                    "2pt"
                },
                ItemsElementName = new[] {
                    ItemsChoiceType4.PaddingLeft,
                    ItemsChoiceType4.PaddingRight,
                    ItemsChoiceType4.PaddingTop,
                    ItemsChoiceType4.PaddingBottom,
                }
            };
        }

        private static IEnumerable<TextRunType> CreateTextRun(string text)
        {
            var resultStyle = new StyleType
            {
                Items = new object[] {
                    "Calibri",
                    "16pt",
                    "#1b75cd"
                },
                ItemsElementName = new[] {
                    ItemsChoiceType4.FontFamily,
                    ItemsChoiceType4.FontSize,
                    ItemsChoiceType4.Color,
                }
            };
            return new List<TextRunType> {
                new TextRunType {
                    Items = new object[] {
                        new LocIDStringWithDataTypeAttribute {
                            Value = text
                        },
                        resultStyle
                    },
                    ItemsElementName = new [] {
                        ItemsChoiceType11.Value,
                        ItemsChoiceType11.Style,
                    }
                }
            };
        }

        private ParagraphsType CreateHeaderTextBoxParagraphs()
        {
            var result = new List<ParagraphType>();
            if (TituloHeader == null)
            {
                return new ParagraphsType
                {
                    Paragraph = result.ToArray()
                };
            }
            if (TituloHeader.Count <= 0)
            {
                return new ParagraphsType
                {
                    Paragraph = result.ToArray()
                };
            }
            result.AddRange(TituloHeader.Select(item => new ParagraphType
            {
                Items = new object[] {
                    new TextRunsType {
                        TextRun = CreateTextRun(item).ToArray()
                    },
                    new StyleType {
                        Items = new [] {
                            "Center"
                        },
                        ItemsElementName = new [] {
                            ItemsChoiceType4.TextAlign,
                        }
                    }
                },
                ItemsElementName = new[] {
                    ItemsChoiceType12.TextRuns,
                    ItemsChoiceType12.Style,
                }
            }));
            return new ParagraphsType
            {
                Paragraph = result.ToArray()
            };
        }

        private static StyleType CreateHeaderStyle()
        {
            return new StyleType
            {
                Items = new object[] {
                    CreateHeaderBackgroundImage()
                },
                ItemsElementName = new[] {
                    ItemsChoiceType4.BackgroundImage,
                }
            };
        }

        private static BackgroundImageType CreateHeaderBackgroundImage()
        {
            return new BackgroundImageType
            {
                Items = new object[] {
                    "Clip",
                    BackgroundImageTypeSource.Embedded,
                    "Cabecalho"
                },
                ItemsElementName = new[] {
                    ItemsChoiceType3.BackgroundRepeat,
                    ItemsChoiceType3.Source,
                    ItemsChoiceType3.Value
                }
            };
        }
        #endregion Header

        #region Footer
        public PageSectionType CreatePageFooter()
        {
            var footerBackground = new BackgroundImageType
            {
                Items = new object[] {
                    "Clip",
                    BackgroundImageTypeSource.Embedded,
                    "Cabecalho"
                },
                ItemsElementName = new[] {
                    ItemsChoiceType3.BackgroundRepeat,
                    ItemsChoiceType3.Source,
                    ItemsChoiceType3.Value
                }
            };
            var footerStyle = new StyleType
            {
                Items = new object[] {
                    footerBackground
                },
                ItemsElementName = new[] {
                    ItemsChoiceType4.BackgroundImage,
                }
            };

            return new PageSectionType
            {
                Items = new object[] {
                    "1.58749cm",
                    true,
                    true,
                    CreateFooterReportItems(),
                    footerStyle,
                },
                ItemsElementName = new[] {
                    ItemsChoiceType76.Height,
                    ItemsChoiceType76.PrintOnFirstPage,
                    ItemsChoiceType76.PrintOnLastPage,
                    ItemsChoiceType76.ReportItems,
                    ItemsChoiceType76.Style,
                }
            };
        }

        private static ReportItemsType CreateFooterReportItems()
        {
            var result = new List<ParagraphType> {
                new ParagraphType {
                    Items = new object[] {
                        new TextRunsType {
                            TextRun = (new List<TextRunType> {
                                new TextRunType {
                                    Items = new object[] {
                                        new LocIDStringWithDataTypeAttribute {
                                            Value = "COPYRIGHT ©2014 - UNIVERSIDADE CORPORATIVA SEBRAE"
                                        },
                                        new StyleType {
                                            Items = new object[] {
                                                "Calibri",
                                                "9pt",
                                                "#1b75cd"
                                            },
                                            ItemsElementName = new[] {
                                                ItemsChoiceType4.FontFamily,
                                                ItemsChoiceType4.FontSize,
                                                ItemsChoiceType4.Color,
                                            }
                                        }
                                    },
                                    ItemsElementName = new [] {
                                        ItemsChoiceType11.Value,
                                        ItemsChoiceType11.Style,
                                    }
                                }
                            }).ToArray()
                        },
                        new StyleType {
                            Items = new object[] {
                                "Center"
                            },
                            ItemsElementName = new [] {
                                ItemsChoiceType4.TextAlign,
                            }
                        }
                    },
                    ItemsElementName = new[] {
                        ItemsChoiceType12.TextRuns,
                        ItemsChoiceType12.Style
                    }
                },
                new ParagraphType {
                    Items = new object[] {
                        new TextRunsType {
                            TextRun = (new List<TextRunType> {
                                new TextRunType {
                                    Items = new object[] {
                                        new LocIDStringWithDataTypeAttribute {
                                            Value = "DATA GERAÇÃO: "+(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
                                        },
                                        new StyleType {
                                            Items = new object[] {
                                                "Calibri",
                                                "7pt",
                                                "#1b75cd"
                                            },
                                            ItemsElementName = new[] {
                                                ItemsChoiceType4.FontFamily,
                                                ItemsChoiceType4.FontSize,
                                                ItemsChoiceType4.Color,
                                            }
                                        }
                                    },
                                    ItemsElementName = new [] {
                                        ItemsChoiceType11.Value,
                                        ItemsChoiceType11.Style,
                                    }
                                }
                            }).ToArray()
                        },
                        new StyleType {
                            Items = new object[] {
                                "Center"
                            },
                            ItemsElementName = new [] {
                                ItemsChoiceType4.TextAlign,
                            }
                        }
                    },
                    ItemsElementName = new[] {
                        ItemsChoiceType12.TextRuns,
                        ItemsChoiceType12.Style
                    }
                }
            };

            var textBoxParagraphs = new ParagraphsType
            {
                Paragraph = result.ToArray()
            };
            var textBoxStyle = new StyleType
            {
                Items = new object[] {
                    "2pt",
                    "2pt",
                    "2pt",
                    "2pt"
                },
                ItemsElementName = new[] {
                    ItemsChoiceType4.PaddingLeft,
                    ItemsChoiceType4.PaddingRight,
                    ItemsChoiceType4.PaddingTop,
                    ItemsChoiceType4.PaddingBottom,
                }
            };
            var textBox = new TextboxType
            {
                Name = "textBox" + Guid.NewGuid().ToString().Split('-')[0],
                Items = new object[] {
                    textBoxParagraphs,
                    "0.30854cm",
                    "0.72836cm",
                    "0.9175cm",
                    textBoxStyle
                },
                ItemsElementName = new[] {
                    ItemsChoiceType14.Paragraphs,
                    ItemsChoiceType14.Top,
                    ItemsChoiceType14.Left,
                    ItemsChoiceType14.Height,
                    ItemsChoiceType14.Style
                }
            };

            return new ReportItemsType
            {
                Items = new object[] {
                    textBox
                }
            };
        }
        #endregion Footer

        private static DataSourcesType CreateDataSources()
        {
            var dataSources = new DataSourcesType
            {
                DataSource = new[] {
                    CreateDataSource()
                }
            };
            return dataSources;
        }
        private static DataSourceType CreateDataSource()
        {
            var dataSource = new DataSourceType
            {
                Name = "SebraeAcademicoBPRelatorios",
                Items = new object[] {
                    CreateConnectionProperties()
                }
            };
            return dataSource;
        }
        private static ConnectionPropertiesType CreateConnectionProperties()
        {
            var connectionProperties = new ConnectionPropertiesType
            {
                Items = new object[] {
                    "/* Local Connection */",
                    "System.Data.DataSet"
                },
                ItemsElementName = new[] {
                    ItemsChoiceType.ConnectString,
                    ItemsChoiceType.DataProvider,
                }
            };
            return connectionProperties;
        }
        private BodyType CreateBody()
        {
            var body = new BodyType
            {
                Items = new object[] {
                    CreateReportItems(),
                    "1.13578in"
                }
            };
            return body;
        }
        private ReportItemsType CreateReportItems()
        {
            var reportItems = new ReportItemsType
            {
                Items = new object[] {
                   CreateTablix()
                }
            };
            return reportItems;
        }

        private TablixType CreateTablix()
        {
            return new TablixType
            {
                Name = "table" + Guid.NewGuid().ToString().Split('-')[0],
                Items = new object[] {
                    CreateTablixBody(),
                    CreateHierarchyColumn(),
                    CreateHierarchyRow(),
                    "Nenhum registro encontrado com os parâmetros informados.",
                    "DataSet1",
                    "1.34091cm",
                    "0cm",
                    "0.2cm",
                    "27.62005cm"
                },
                ItemsElementName = new[] {
                    ItemsChoiceType73.TablixBody,
                    ItemsChoiceType73.TablixColumnHierarchy,
                    ItemsChoiceType73.TablixRowHierarchy,
                    ItemsChoiceType73.NoRowsMessage,
                    ItemsChoiceType73.DataSetName,
                    ItemsChoiceType73.Top,
                    ItemsChoiceType73.Left,
                    ItemsChoiceType73.Height,
                    ItemsChoiceType73.Width,
                }
            };
        }

        private TablixHierarchyType CreateHierarchyColumn()
        {
            return new TablixHierarchyType
            {
                Items = new object[] {
                    new TablixMembersType {
                        TablixMember = Fields.Select(item => new TablixMemberType {
                            Items = new object[] {
                                new VisibilityType {
                                    Items = new object[] {
                                        "="+(!item.Visible)
                                    },
                                    ItemsElementName = new[] {
                                        ItemsChoiceType7.Hidden,
                                    }
                                }
                            },
                            ItemsElementName = new[] {
                                ItemsChoiceType72.Visibility,
                            }
                        }).ToArray()
                    }
                }
            };
        }

        private static TablixHierarchyType CreateHierarchyRow()
        {
            return new TablixHierarchyType
            {
                Items = new object[] {
                    new TablixMembersType {
                        TablixMember = new[] {
                            new TablixMemberType {
                                Items = new object[] {
                                    TablixMemberTypeKeepWithGroup.After,
                                    true
                                },
                                ItemsElementName = new [] {
                                    ItemsChoiceType72.KeepWithGroup,
                                    ItemsChoiceType72.RepeatOnNewPage,
                                }
                            },
                            new TablixMemberType {
                                Items = new object[] {
                                    TablixMemberTypeKeepWithGroup.After,
                                    true
                                },
                                ItemsElementName = new [] {
                                    ItemsChoiceType72.KeepWithGroup,
                                    ItemsChoiceType72.RepeatOnNewPage,
                                }
                            },
                            new TablixMemberType {
                                Items = new object[] {
                                    new GroupType {
                                        Name = "Details"
                                    }
                                },
                                ItemsElementName = new [] {
                                    ItemsChoiceType72.Group,
                                }
                            }
                        }
                    }
                }
            };
        }
        private TablixBodyType CreateTablixBody()
        {
            return new TablixBodyType
            {
                Items = new object[] {
                    CreateTablixColumns(),
                    CreateTablixRows()
                }
            };
        }

        private TablixColumnsType CreateTablixColumns()
        {
            var list = new List<object>();
            if (Fields != null && Fields.Count > 0)
            {
                list.AddRange(Fields.Select(item => new TablixColumnType
                {
                    Items = new[] {
                        item.Width.ToString().Replace(",", ".") + "cm"
                    }
                }));
            }
            return new TablixColumnsType
            {
                Items = list.ToArray()
            };
        }
        private TablixRowsType CreateTablixRows()
        {
            var listRows = new List<object> ();
            if(Fields.Any(p => !string.IsNullOrEmpty(p.GroupBy)))
            {
                listRows.Add(CreateTablixHeaderGroup());
            }
            listRows.Add(CreateTablixHeader());
            listRows.Add(CreateTablixRowsContent());

            return new TablixRowsType
            {
                Items = listRows.ToArray()
            };
        }

        private IList<object> CreateTablixCellTypeGroup(string value,int colspan)
        {
            var resultado = new List<object>();

            resultado.Add(new TablixCellType
            {
                Items = new object[]
                    {
                        new CellContentsType
                        {
                            Items = new object[] 
                            {
                                CreateTablixTextBoxHeader(value),
                                (uint)colspan
                            },
                            ItemsElementName = new[] 
                            {
                                ItemsChoiceType71.Textbox,
                                ItemsChoiceType71.ColSpan, 
                            }
                        }
                    }
            });

            for (var i = 1; i < colspan; i++)
            {
                resultado.Add(new TablixCellType
                {
                    Items = new object[] { }
                });
            }

            return resultado;
        }

        private TablixRowType CreateTablixHeaderGroup()
        {
            var resultado = new List<object>();
            var index = 0;
            var lsGrupo = new List<string>();
            for(var saltos = 0; saltos < Fields.Count; saltos++)
            {
                var grupo = "";
                var colspan = 0;
                var camposVisiveis = Fields;//.Where(p => p.Visible).ToList();
                for (var i = index; i < camposVisiveis.Count; i++)
                {
                    index = i; // sempre atualizar o index;
                    var item = camposVisiveis[i];
                    if (string.IsNullOrEmpty(item.GroupBy))
                    {
                        colspan++;
                        continue;
                    }
                    //encontrou algum grupo de colunas
                    grupo = item.GroupBy;
                    break;
                }

                if(colspan > 0) resultado.AddRange(CreateTablixCellTypeGroup(null,colspan));

                if (index >= camposVisiveis.Count || !camposVisiveis.Any(p => !string.IsNullOrEmpty(p.GroupBy) && !lsGrupo.Contains(p.GroupBy))) continue;

                colspan = camposVisiveis.Count(p => p.GroupBy == grupo);

                lsGrupo.Add(grupo);

                resultado.AddRange(CreateTablixCellTypeGroup(grupo, colspan));

                index += colspan;

                saltos = index;
            };

            return new TablixRowType
            {
                Items = new object[] 
                {
                    "0.6cm",
                    new TablixCellsType
                    {
                        Items = resultado.ToArray()
                    }
                }
            };
        }

        private TablixRowType CreateTablixHeader()
        {
            return new TablixRowType
            {
                Items = new object[]
                {
                    "0.6cm",
                    new TablixCellsType
                    {
                        Items = Fields.Select(item => new TablixCellType
                        {
                            Items = new object[] 
                            {
                                new CellContentsType
                                {
                                    Items = new object[] 
                                    {
                                        CreateTablixTextBoxHeader(item.Text)
                                    },
                                    ItemsElementName = new[] 
                                    {
                                        ItemsChoiceType71.Textbox
                                    }
                                }
                            }
                        }).Cast<object>().ToArray()
                    }
                }
            };
        }

        private TablixRowType CreateTablixRowsContent()
        {
            return new TablixRowType
            {
                Items = new object[] {
                    "0.23622in",
                    new TablixCellsType {
                        Items = Fields.Select(item => new TablixCellType {
                            Items = new object[] {
                                new CellContentsType {
                                    Items = new object[] {
                                        CreateTablixTextBoxField(item)
                                    },
                                    ItemsElementName = new[] {
                                        ItemsChoiceType71.Textbox
                                    }
                                }
                            }
                        }).Cast<object>().ToArray()
                    }
                }
            };
        }

        private static TextboxType CreateTablixTextBoxField(RdlColumnHeader item)
        {
            var textBoxStyle = new StyleType
            {
                Items = new object[]
                {
                    "2pt",
                    "2pt",
                    "2pt",
                    "2pt",
                    new BorderType
                    {
                        Items = new object[] 
                        {
                            "Solid",
                            "LightGrey"
                        },
                        ItemsElementName = new [] 
                        {
                            ItemsChoiceType2.Style,
                            ItemsChoiceType2.Color,
                        }
                    }
                },
                ItemsElementName = new[] {
                    ItemsChoiceType4.PaddingLeft,
                    ItemsChoiceType4.PaddingRight,
                    ItemsChoiceType4.PaddingTop,
                    ItemsChoiceType4.PaddingBottom,
                    ItemsChoiceType4.Border,
                }
            };
            var textBoxParagraphs = new ParagraphsType
            {
                Paragraph = (new List<ParagraphType>
                {
                    new ParagraphType
                    {
                        Items = new object[] 
                        {
                            new TextRunsType
                            {
                                TextRun = (new List<TextRunType>
                                {
                                    new TextRunType
                                    {
                                        Items = new object[] 
                                        {
                                            new LocIDStringWithDataTypeAttribute
                                            {
                                                Value = "=Fields!"+item.Value+".Value"
                                            },
                                            new StyleType
                                            {
                                                Items = new object[] 
                                                {
                                                    "8pt"
                                                },
                                                ItemsElementName = new [] 
                                                {
                                                    ItemsChoiceType4.FontSize
                                                }
                                            }
                                        },
                                        ItemsElementName = new [] 
                                        {
                                            ItemsChoiceType11.Value,
                                            ItemsChoiceType11.Style,
                                        }
                                    }
                                }).ToArray()
                            }
                        },
                        ItemsElementName = new[] 
                        {
                            ItemsChoiceType12.TextRuns,
                            ItemsChoiceType12.Style,
                        }
                    }
                }).ToArray()
            };
            return new TextboxType
            {
                Name = item.Value,
                Items = new object[] 
                {
                    true,
                    true,
                    textBoxParagraphs,
                    textBoxStyle
                },
                ItemsElementName = new[] 
                {
                    ItemsChoiceType14.CanGrow,
                    ItemsChoiceType14.KeepTogether,
                    ItemsChoiceType14.Paragraphs,
                    ItemsChoiceType14.Style,
                }
            };
        }

        private static TextboxType CreateTablixTextBoxHeader(string value)
        {
            var textBoxStyle = new StyleType
            {
                Items = new object[] 
                {
                    "2pt",
                    "2pt",
                    "2pt",
                    "2pt",
                    "#1b75cd",
                    new BorderType
                    {
                        Items = new object[] 
                        {
                            "Solid",
                            "LightGrey"
                        },
                        ItemsElementName = new [] 
                        {
                            ItemsChoiceType2.Style,
                            ItemsChoiceType2.Color,
                        }
                    }
                },
                ItemsElementName = new[] 
                {
                    ItemsChoiceType4.PaddingLeft,
                    ItemsChoiceType4.PaddingRight,
                    ItemsChoiceType4.PaddingTop,
                    ItemsChoiceType4.PaddingBottom,
                    ItemsChoiceType4.BackgroundColor,
                    ItemsChoiceType4.Border,
                }
            };
            var textBoxParagraphs = new ParagraphsType
            {
                Paragraph = (new List<ParagraphType>
                {
                    new ParagraphType
                    {
                        Items = new object[]
                        {
                            new TextRunsType
                            {
                                TextRun = (new List<TextRunType>
                                {
                                    new TextRunType
                                    {
                                        Items = new object[] 
                                        {
                                            new LocIDStringWithDataTypeAttribute
                                            {
                                                Value = value
                                            },
                                            new StyleType
                                            {
                                                Items = new object[] 
                                                {
                                                    "White",
                                                    "Bold"
                                                },
                                                ItemsElementName = new []
                                                {
                                                    ItemsChoiceType4.Color,
                                                    ItemsChoiceType4.FontWeight,
                                                }
                                            }
                                        },
                                        ItemsElementName = new [] {
                                            ItemsChoiceType11.Value,
                                            ItemsChoiceType11.Style,
                                        }
                                    }
                                }).ToArray()
                            },
                            new StyleType
                            {
                                Items = new object[]
                                {
                                    "Center"
                                },
                                ItemsElementName = new [] 
                                {
                                    ItemsChoiceType4.TextAlign,
                                }
                            }
                        },
                        ItemsElementName = new[]
                        {
                            ItemsChoiceType12.TextRuns,
                            ItemsChoiceType12.Style,
                        }
                    }
                }).ToArray()
            };
            return new TextboxType
            {
                Name = "textBox" + Guid.NewGuid().ToString().Split('-')[0],
                Items = new object[] 
                {
                    true,
                    true,
                    textBoxParagraphs,
                    textBoxStyle
                },
                ItemsElementName = new[] 
                {
                    ItemsChoiceType14.CanGrow,
                    ItemsChoiceType14.KeepTogether,
                    ItemsChoiceType14.Paragraphs,
                    ItemsChoiceType14.Style,
                }
            };
        }

        private DataSetsType CreateDataSets()
        {
            var dataSets = new DataSetsType
            {
                DataSet = new[] {
                    CreateDataSet()
                }
            };
            return dataSets;
        }
        private DataSetType CreateDataSet()
        {
            var dataSet = new DataSetType
            {
                Name = "DataSet1",
                Items = new object[] {
                    CreateFields(),
                    CreateQuery()
                }
            };
            return dataSet;
        }
        private static QueryType CreateQuery()
        {
            var query = new QueryType
            {
                Items = new object[] {
                    "SebraeAcademicoBPRelatorios",
                    "/* Local Query */",
                },
                ItemsElementName = new[] {
                    ItemsChoiceType1.DataSourceName,
                    ItemsChoiceType1.CommandText,
                }
            };
            return query;
        }
        private FieldsType CreateFields()
        {
            return new FieldsType
            {
                Field = Fields.Select(item => new FieldType
                {
                    Name = item.Value,
                    Items = new object[] {
                        item.Value
                    }
                }).ToArray()
            };
        }
        public void WriteXml(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(Report));
            serializer.Serialize(stream, CreateReport());
        }
    }
}
