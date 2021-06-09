using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casa
{

    private Vector3 pos;
    private char tipo; // . C x S
    protected GameObject cubo;

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

    public void jaConheco()
    {
        cubo.GetComponentInChildren<Renderer>().material.color = Color.yellow;
    }




    public override string ToString()
    {
        string aux = "Existo " + pos + " e do tipo " + tipo;
        return aux;
    }



}