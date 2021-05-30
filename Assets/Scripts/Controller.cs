using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Controller : MonoBehaviour
{
    string[] linhas;
    //Casa[] casas;   // lista de nodos
    Casa[,] nodos;
    //set de arrestas
    HashSet<Casa[]> arrestas;

    Vector3 cavalo;
    Vector3 saida;



    // Start is called before the first frame update
    void Start()
    {
        this.linhas = System.IO.File.ReadAllLines(@".\Assets\Casos\caso50.txt");
        GeraListaCasas();
        arrestas = new HashSet<Casa[]>();
        GeraArrestas();
        List<Casa> jaConheco = new List<Casa>();
        Debug.Log("Cavalo " + cavalo);
        Debug.Log("Saida " + saida);
        caminhamentoLargura(nodos[(int)cavalo.z, (int) cavalo.x], nodos[(int)saida.z, (int)cavalo.x], jaConheco);
    }

    void GeraListaCasas()
    {
        // gera o tabuleiro passando as posições como coordenadas
        //casas = new Casa[linhas[0].Length * linhas.Length];
        this.nodos = new Casa[linhas.Length, linhas[0].Length]; // acabei fazendo

        // create shape
        int i = 0;

        for (int x = 0; x < linhas[0].Length; x++)
        {
            for (int z = 0; z < linhas.Length; z++)
            {
                Casa c = new Casa(new Vector3(x, 0, z), linhas[z][x]);
                //casas[i] = c;
                nodos[z,x] = c; 
                CreateCubes(new Vector3(x, 0, z), linhas[z][x]);

                i++;
            }
        }
    }

    private void CreateCubes(Vector3 pos, char tipo)
    {
        GameObject cubo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo.transform.position = pos;
        cubo.transform.parent = this.transform;
        
        if(tipo == 'x')
            cubo.GetComponentInChildren<Renderer>().material.color = Color.red;
        if (tipo == 'C')
        {
            cubo.GetComponentInChildren<Renderer>().material.color = Color.black;
            cavalo = pos;
        }

        if (tipo == 'S')
        {
            cubo.GetComponentInChildren<Renderer>().material.color = Color.green;
            saida = pos;
        }

        nodos[(int)pos.z, (int)pos.x].setCubo(cubo);

    }

    /*
    * Cada casa deve apontar para o equivalente aos "A"s 
    *   doisEsq umEsq umDir doisDir
    * . . A . A . . doisCima
    * . A . . . A . umCima
    * . . . C . . . 
    * . A . . . A . umBaixo
    * . . A . A . . doisBaixo
    */


    private void GeraArrestas()
    {
        for (int x = 0; x < linhas[0].Length; x++) // x = colunas
        {


            int doisDir = x + 2;
            int umDir = x + 1;
            if (doisDir >= linhas[0].Length) // 0 - 9 // 9+2=11 8+2=10
            {
                if (doisDir == linhas[0].Length) // 10 dois=10     9 0
                {
                    doisDir = 0;
                }
                else if (doisDir == linhas[0].Length + 1)   // 11
                {
                    doisDir = 1;        // 11 -> 1
                    umDir = 0;          // 10 -> 0
                }
            }

            int doisEsq = x - 2;
            int umEsq = x - 1;
            if (doisEsq < 0)
            {
                if (doisEsq == 0)
                {
                    umEsq = linhas[0].Length - 1;
                }
                else if (doisEsq == -1)
                {
                    doisEsq = linhas[0].Length - 1;
                }
                else if (doisEsq == -2)
                {
                    doisEsq = linhas[0].Length - 2;
                    umEsq = linhas[0].Length - 1;
                }
            }

            //Debug.Log(x + "\nDoisEsq: " + doisEsq + "\tUmCima: " + umEsq
            //    + "\nDoisDir: " + doisDir + "\tUmDir: " + umDir);

            for (int z = 0; z < linhas.Length; z++)
            {
                // se o nodo for x ele nem precisa gerar os caminhos
                //if (nodos[z,x].getTipo() != 'x')
               // {
                    int doisCima = z + 2;
                    int umCima = z + 1;
                    // z vai ate linhas.length - 1 
                    // linhas.length -1 +2 = l.l+1
                    if (doisCima >= linhas.Length)
                    {
                        if (doisCima == linhas.Length)
                        {
                            doisCima = 0;
                        }
                        else if (doisCima == linhas.Length + 1)
                        {
                            doisCima = 1;
                            umCima = 0;
                        }
                    }

                    int doisBaixo = z - 2;
                    int umBaixo = z - 1;
                    // z começa em 0
                    // 0 -2 = -2
                    if (doisBaixo < 0)
                    {
                        if (doisBaixo == 0)
                        {
                            umBaixo = linhas.Length - 1;
                        }
                        else if (doisBaixo == -1)
                        {
                            doisBaixo = linhas.Length - 1;
                        }
                        else if (doisBaixo == -2)
                        {
                            doisBaixo = linhas.Length - 2;
                            umBaixo = linhas.Length - 1;
                        }
                    }

                    //Debug.Log(z + "\nDoisCima: " + doisCima + "\tUmCima: " + umCima
                    //    + "\nDoisBaixo: " + doisBaixo + "\tUmBaixo: " + umBaixo);

                    //verificar se é x

                    Casa[] dupla = new Casa[2];
                    dupla[0] = nodos[z, x];

                    Casa temp = nodos[doisCima, umEsq];
                    if (temp.getTipo() != 'x')
                    {
                        dupla[1] = temp;
                        arrestas.Add(dupla);
                    }

                    temp = nodos[umCima, doisEsq];
                    if (temp.getTipo() != 'x')
                    {
                        dupla[1] = temp;
                        arrestas.Add(dupla);
                    }

                    temp = nodos[doisCima, umDir];
                    if (temp.getTipo() != 'x')
                    {
                        dupla[1] = temp;
                        arrestas.Add(dupla);
                    }

                    temp = nodos[umCima, doisDir];
                    if (temp.getTipo() != 'x')
                    {
                        dupla[1] = temp;
                        arrestas.Add(dupla);
                    }

                    temp = nodos[umBaixo, doisEsq];
                    if (temp.getTipo() != 'x')
                    {
                        dupla[1] = temp;
                        arrestas.Add(dupla);
                    }

                    temp = nodos[doisBaixo, umEsq];
                    if (temp.getTipo() != 'x')
                    {
                        dupla[1] = temp;
                        arrestas.Add(dupla);
                    }

                    temp = nodos[doisBaixo, umDir];
                    if (temp.getTipo() != 'x')
                    {
                        dupla[1] = temp;
                        arrestas.Add(dupla);
                    }

                    temp = nodos[umBaixo, doisDir];
                    if (temp.getTipo() != 'x')
                    {
                        dupla[1] = temp;
                        arrestas.Add(dupla);
                    }
               // }
            }
        }
    }
          
    private Boolean caminhamentoLargura(Casa inicial, Casa final, List<Casa> jaConheco)
    {
        jaConheco.Add(inicial);
        inicial.jaConheco();
        Thread.Sleep(100);


        foreach (Casa vertice in nodos)
        {
            int existeArresta = Array.IndexOf(new Casa[] { inicial, vertice}, arrestas); // descobrir como fazer a comparação se existe a arresta
            if (existeArresta > 1)
            {
                if (caminhamentoLargura(vertice, final, jaConheco))
                {
                    return true;
                }
            }
        }
        Debug.Log("Não tem caminho");
        return false;
    }



}