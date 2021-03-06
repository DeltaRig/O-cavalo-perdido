using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casa
{

    private Vector3 pos;
    private char tipo; // . C x S
    protected GameObject cubo;
    private int dist;
    private string cor;
    private List<int[]> arrestas;
    private Casa filho;

    /*
     * Cada casa deve apontar para o equivalente aos "A"s 
     * 
     * . . A . A . .
     * . A . . . A .
     * . . . C . . . 
     * . A . . . A .
     * . . A . A . .
     */


    public Casa(Vector3 pos, char tipo)
    {
        this.tipo = tipo;
        this.pos = pos;
        this.dist = 99999;
        this.cor = "BRANCO";
        this.arrestas = new List<int[]>();
        
    }

    public Vector3 getPos()
    {
        return pos;
    }

    public char getTipo()
    {
        return tipo;
    }

    public void setCubo(GameObject cubo)
    {
        this.cubo = cubo;
    }

    public void setDist(int dist)
    {
        this.dist = dist;
    }

    public int getDist()
    {
        return dist;
    }

    public string getCor()
    {
        return cor;
    }

    public List<int[]> getArrestas()
    {
        return arrestas;
    }

    public void addArresta(int[] arresta)
    {
        this.arrestas.Add(arresta);
    }


    public void jaConheco()
    {
        cubo.GetComponentInChildren<Renderer>().material.color = Color.black;
        this.cor = "PRETO";
    }

    public void fronteira()
    {
        cubo.GetComponentInChildren<Renderer>().material.color = Color.cyan;
        this.cor = "AZUL";
    }

    public void setCor(string cor)  //para usar no optimize
    {
        this.cor = cor;
    }

    public void setFilho(Casa filho)  //para usar no optimize
    {
        this.filho = filho;
    }

    public Casa getFilho()
    {
        return filho;
    }

    public void caminhho()
    {
        cubo.GetComponentInChildren<Renderer>().material.color = Color.green;
    }

    public override string ToString()
    {
        string aux = "Existo " + pos + " e do tipo " + tipo;
        return aux;
    }



}