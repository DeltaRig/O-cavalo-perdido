using System;
using System.Collections;
using System.Collections.Generic;
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

        //Debug.Log("Cavalo " + cavalo);
        //Debug.Log("Saida " + saida);

        StartCoroutine(CaminhamentoLargura(nodos[(int)cavalo.z, (int)cavalo.x], nodos[(int)saida.z, (int)saida.x]));
        

    }

    void GeraListaCasas()
    {
        // gera o tabuleiro passando as posições como coordenadas
        //casas = new Casa[linhas[0].Length * linhas.Length];
        this.nodos = new Casa[linhas.Length, linhas[0].Length]; // acabei fazendo

        // create casas
        for (int x = 0; x < linhas[0].Length; x++)
        {
            for (int z = 0; z < linhas.Length; z++)
            {
                Casa c = new Casa(new Vector3(x, 0, z), linhas[z][x]);
                nodos[z, x] = c;
                CreateCubes(new Vector3(x, 0, z), linhas[z][x]);

            }
        }
    }

    private void CreateCubes(Vector3 pos, char tipo)
    {
        GameObject cubo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo.transform.position = pos;
        cubo.transform.parent = this.transform;

        if (tipo == 'x')
            cubo.GetComponentInChildren<Renderer>().material.color = Color.red;
        else if (tipo == 'C')
        {
            cubo.GetComponentInChildren<Renderer>().material.color = Color.yellow;
            cavalo = pos;
        } else if (tipo == 'S')
        {
            cubo.GetComponentInChildren<Renderer>().material.color = Color.green;
            saida = pos;
        } else
        {
            cubo.GetComponentInChildren<Renderer>().material.color = Color.white;
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

    private void GeraArrestasS()
    {
        int[,] mov = new int[,] { { 2, 1 }, { 1, 2 }, { 2, -1 }, { 1, -2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 } };

        for (int x = 0; x < linhas[0].Length; x++) // x = colunas
        {
            for (int z = 0; z < linhas.Length; z++)
            {
                if (nodos[z, x].getTipo() != 'x')
                    for(int i = 0; i < mov.Length; i++)
                    {
                        int horizontal = (int) nodos[z, x].getPos().x + mov[i,0];
                        int vertical = (int) nodos[z, x].getPos().z + mov[i,1];

                        if (horizontal >= nodos.Length)
                        {
                            if (horizontal == nodos.Length)
                                horizontal = 0;
                            else if (horizontal == nodos.Length + 1)
                                horizontal = 1;
                        }
                        else if (horizontal < 0)
                        {
                            if (horizontal == -1)
                                horizontal = nodos.Length - 1;
                            else if (horizontal == -2)
                                horizontal = nodos.Length - 2;
                        }
                        if (vertical >= linhas[0].Length)
                        {
                            if (vertical == linhas[0].Length)
                                vertical = 0;
                            else if (vertical == linhas[0].Length + 1)
                                vertical = 1;
                        }
                        else if (vertical < 0)
                        {
                            if (vertical == -1)
                                vertical = linhas[0].Length - 1;
                            else if (vertical == -2)
                                vertical = linhas[0].Length - 2;
                        }

                        Debug.Log("vert " + vertical + "hor " + horizontal);

                        if (nodos[vertical, horizontal].getTipo() != 'x')
                        {
                            arrestas.Add(new Casa[] { nodos[z, x], nodos[vertical, horizontal] });
                        }
                    }

            }
        }
    }

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

                
                if (nodos[z, x].getTipo() != 'x')
                {
                    Casa temp = nodos[doisCima, umEsq];
                    if (temp.getTipo() != 'x')
                    {
                        arrestas.Add(new Casa[] { nodos[z, x] , nodos[doisCima, umEsq] });
                    }

                    temp = nodos[umCima, doisEsq];
                    if (temp.getTipo() != 'x')
                    {
                        arrestas.Add(new Casa[] { nodos[z, x], nodos[umCima, doisEsq] });
                    }

                    temp = nodos[doisCima, umDir];
                    if (temp.getTipo() != 'x')
                    {
                        arrestas.Add(new Casa[] { nodos[z, x], nodos[doisCima, umDir] });
                    }

                    temp = nodos[umCima, doisDir];
                    if (temp.getTipo() != 'x')
                    {
                        arrestas.Add(new Casa[] { nodos[z, x], nodos[umCima, doisDir] });
                    }

                    temp = nodos[umBaixo, doisEsq];
                    if (temp.getTipo() != 'x')
                    {
                        arrestas.Add(new Casa[] { nodos[z, x], nodos[umBaixo, doisEsq] });
                    }

                    temp = nodos[doisBaixo, umEsq];
                    if (temp.getTipo() != 'x')
                    {
                        arrestas.Add(new Casa[] { nodos[z, x], nodos[doisBaixo, umEsq] });
                    }

                    temp = nodos[doisBaixo, umDir];
                    if (temp.getTipo() != 'x')
                    {
                        arrestas.Add(new Casa[] { nodos[z, x], nodos[doisBaixo, umDir] });
                    }

                    temp = nodos[umBaixo, doisDir];
                    if (temp.getTipo() != 'x')
                    {
                        arrestas.Add(new Casa[] { nodos[z, x], nodos[umBaixo, doisDir] });
                    }
                }
            }
        }
    }

    private Boolean caminhamento(Casa casaInicial, Casa casaFinal, List<Casa> casasConhecida)
    {
        if (casaInicial == casaFinal)
        {  // nao ocorre
            Debug.Log("distancia" + casaFinal.getDist());
            return true;
        }

        casasConhecida.Add(casaInicial);    // adiciona na lista
        casaInicial.jaConheco();
        

        foreach (Casa casa in nodos)
        {
            if (!existeNodo(casasConhecida, casa))
            {
                if (existeArresta(casaInicial, casa))
                {
                    casa.setDist(casaInicial.getDist() + 1);
                    Debug.Log("" + casaInicial.getPos() + " dist " + casaFinal.getDist() + " | "+ casaFinal.getPos() );
                    if (caminhamento(casa, casaFinal, casasConhecida))
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }

    IEnumerator CaminhamentoLargura(Casa casaInicial, Casa casaFinal)
    {
        yield return new WaitForSeconds(2);
        //foreach(Casa c in nodos) c.setDist(99999);    Definido na criação do objeto
        casaInicial.setDist(0);

        List<Casa> queue = new List<Casa> { };
        queue.Add(casaInicial);

        Casa casaAtual = casaInicial;
        while(!equalNode(casaFinal, casaAtual) & queue.Count > 0){
            casaAtual = queue[0];
            queue.RemoveAt(0);

            foreach (Casa v in nodos) 
            {
                if (existeArresta(casaAtual, v)) // para cada arresta adjacente
                {
                    if (v.getCor().Equals("BRANCO"))
                    {
//                        yield return new WaitForSeconds(.0001f);
                        v.fronteira(); //muda a cor para cinza
                        v.setDist(casaAtual.getDist() + 1);
                        v.setParent(casaAtual);
                        queue.Add(v);

                    }
                }
            }
            yield return new WaitForSeconds(.0000000000000001f);
            casaAtual.jaConheco(); //muda a cor para amarelo
        }
        Debug.Log("resultado " + nodos[(int)saida.z, (int)saida.x].getDist());
        if (queue.Count == 0)
        {
            Debug.Log("Explorei todos nodos possiveis");
        }

        yield return new WaitForSeconds(.0000000000000000000001f);

    }



    private Boolean existeArresta(Casa casaSaida, Casa casaDestino)
    {
        foreach (Casa[] casa in arrestas)
        {
            if (casaSaida.getPos().x == casa[0].getPos().x)
            {
                if (casaSaida.getPos().z == casa[0].getPos().z)
                {
                    if (casaDestino.getPos().x == casa[1].getPos().x)
                    {
                        if (casaDestino.getPos().z == casa[1].getPos().z)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private Boolean existeNodo(List<Casa> casasConhecidas, Casa casa)
    {
        foreach (Casa c in casasConhecidas)
        {
            if (c.getPos().x == casa.getPos().x)
            {
                if (c.getPos().z == casa.getPos().z)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Boolean equalNode(Casa c, Casa casa)
    {

        if (c.getPos().x == casa.getPos().x)
        {
            if (c.getPos().z == casa.getPos().z)
            {
                return true;
            }
        }

        return false;
    }


}