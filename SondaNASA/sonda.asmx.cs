/**
* Arquivo: sondaNASA.asmx.cs
*
* Uma sonda exploradora da NASA pousou em marte. O pouso se deu em uma área retangular,
* na qual a sonda pode navegar usando uma interface web. A posição da sonda é representada
* pelo seu eixo x e y, e a direção que ele está apontado pela letra inicial, 
* sendo as direções válidas:
* E - Esquerda
* D - Direita
* C - Cima
* B - Baixo
* 
* A sonda aceita três comandos:
* GE - girar 90 graus à esquerda
* GD - girar 90 graus à direta
* M - movimentar. Para cada comando M a sonda se move uma posição na direção à qual sua face está apontada.
* 
* A sonda inicia no quadrante (x = 0, y = 0), o que se traduz como a casa mais inferior da esquerda; 
* também inicia com a face para a direita. 
* 
* Esperamos três endpoints, um que envie a sonda para a posição inicial (0,0); 
* outro deve receber o movimento da sonda e responder com as coordenadas finais, 
* caso o movimento seja válido ou erro caso o movimento seja inválido; e o 
* terceiro deve responder apenas com as coordenadas atuais x e y da sonda.
* 
* Consideramos que um movimento para cima é o mesmo que dizer (x + 1, y) e 
* um movimento para a direita é o mesmo que (x, y + 1).
* 
* Cada comando aceita uma série de movimentos que a sonda pode executar. 
* Uma sugestão seria um endpoint que recebe um array com uma sequência de movimentos 
* (mas essa é apenas uma sugestão, fique à vontade para usar outra abordagem caso você 
* encontre um motivo prático para isso).
* 
*
*
* Autor                       Data              Historico de Mudanças\n
* -------------------------   ------------      ----------------------------------------\n
* Thiago Nascimento           2021-07-22        Criação Inicial.\n
*
*/

using System;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace SondaNASA
{

    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private const string MOVE = "M";
        private const string GIRAR_ESQUERDA = "GE";
        private const string GIRAR_DIREITA = "GD";
        private const string FACE_DIREITA = "D";
        private const string FACE_ESQUERDA = "E";
        private const string FACE_CIMA = "C";
        private const string FACE_BAIXO = "B";
        private const int DIMENSAO_X = 5;
        private const int DIMENSAO_Y = 5;

        public static class SondaData
        {
            public static int coordX = 0;
            public static int coordY = 0;
            public static string face = FACE_DIREITA;
            public static string mensagem;

            public static String getStatus()
            {
                return "{x:" + coordX + ",y:" + coordY + ",face:" + face + ", mensagem:" + mensagem + "}";
            }
        }

        [WebMethod]
        // movimentos = "GE|M|M|M|GD|M|M"
        public string MoveSonda(string movimentos)
        {
            int qtdMovimentos;
            int i;
            string[] arrayMovimentos = movimentos.Split('|');
            string eixo = "";

            qtdMovimentos = arrayMovimentos.Length;
            
            for (i = 0; i < qtdMovimentos; i++)
            {
                if (arrayMovimentos[i].Equals(MOVE))
                {
                    try
                    {
                        switch (SondaData.face)
                        {
                            case FACE_DIREITA:

                                if (SondaData.coordX == DIMENSAO_X - 1)
                                {
                                    throw new Exception("Movimento fora da area definida!");
                                }
                                else
                                {
                                    SondaData.coordX++;
                                    eixo = "X";
                                }
                                break;

                            case FACE_ESQUERDA:
                                if (SondaData.coordX == 0)
                                {
                                    throw new Exception("Movimento fora da area definida!");
                                }
                                else
                                {
                                    SondaData.coordX--;
                                    eixo = "X";
                                }
                                break;

                            case FACE_BAIXO:
                                if (SondaData.coordY == 0)
                                {
                                    throw new Exception("Movimento fora da area definida!");
                                }
                                else
                                {
                                    SondaData.coordY--;
                                    eixo = "Y";
                                }
                                break;

                            case FACE_CIMA:
                                if (SondaData.coordY == DIMENSAO_Y - 1)
                                {
                                    throw new Exception("Movimento fora da area definida!");
                                }
                                else
                                {
                                    SondaData.coordY++;
                                    eixo = "Y";
                                }
                                break;
                        }

                        SondaData.mensagem = SondaData.mensagem + ", Sonda se moveu no eixo " + eixo;
                    }
                    catch
                    {

                        //TODO: trata erro movimento fora escopo
                        return new JavaScriptSerializer().Serialize("{erro:Um movimento inválido foi detectado, infelizmente a sonda ainda não possui a habilidade de " + movimentos + "}");
                    }
                }
                else if (arrayMovimentos[i].Equals(GIRAR_DIREITA))
                {
                    switch (SondaData.face)
                    {
                        case FACE_DIREITA:
                            SondaData.face = FACE_BAIXO;
                            break;

                        case FACE_ESQUERDA:
                            SondaData.face = FACE_CIMA;
                            break;

                        case FACE_BAIXO:
                            SondaData.face = FACE_ESQUERDA;
                            break;

                        case FACE_CIMA:
                            SondaData.face = FACE_DIREITA;
                            break;
                    }

                    SondaData.mensagem = SondaData.mensagem + ", Sonda girou a direita";
                }
                else if (arrayMovimentos[i].Equals(GIRAR_ESQUERDA))
                {
                    switch (SondaData.face)
                    {
                        case FACE_DIREITA:
                            SondaData.face = FACE_CIMA;
                            break;

                        case FACE_ESQUERDA:
                            SondaData.face = FACE_BAIXO;
                            break;

                        case FACE_BAIXO:
                            SondaData.face = FACE_DIREITA;
                            break;

                        case FACE_CIMA:
                            SondaData.face = FACE_ESQUERDA;
                            break;
                    }

                    SondaData.mensagem = SondaData.mensagem + ", Sonda girou a esquerda";
                }
            }

            return new JavaScriptSerializer().Serialize(SondaData.getStatus());
        }

        [WebMethod]
        public bool IniciaSonda()
        {
            if(SondaData.coordX != 0)
            {
                SondaData.face = FACE_ESQUERDA;

                for (int i=SondaData.coordX; i > 0; i--)
                {
                    SondaData.coordX--;
                }
            }

            if (SondaData.coordY != 0)
            {
                SondaData.face = FACE_BAIXO;

                for (int i = SondaData.coordY; i > 0; i--)
                {
                    SondaData.coordY--;
                }
            }

            SondaData.face = FACE_DIREITA;
            
            return true;
        }

        [WebMethod]
        public string StatusSonda()
        {
            return new JavaScriptSerializer().Serialize(SondaData.getStatus());
        }
    }
}
