import numpy as np
import time

from casa import Casa

testCases = [50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550] # 
horse = []
exit = []
nodos = []
arestas = set()

def geraListaCasas(lines):
    countLine = 0
    for line in lines:
        countCol = 0
        colunas = []
        for character in line:
            if(character != "\n"):
                pos = (countLine, countCol)
                tipo = character
                if tipo == 'C':
                    global horse
                    horse = pos
                elif tipo == 'S':
                    global exit
                    exit = pos
                c = Casa(pos, tipo)
                colunas.append(c)
                countCol += 1
        nodos.append(colunas)
        countLine += 1

def geraArrestas(c):
    moviments = ((2, 1), (1,2), (2,-1), (1,-2), (-1, 2), (-2,1), (-2,-1), (-1,-2))

    c.startArrestas()
    #print(len(nodos))
    #print(len(nodos[0]))

    if c.getTipo() != 'x':
        for mov in moviments:
            x = c.getX() + mov[0]
            y = c.getY() + mov[1]

            if(x >= len(nodos)):
                if(x == len(nodos)):
                    x = 0
                elif(x == len(nodos) + 1):
                    x = 1
            elif(x < 0):
                if(x == -1):
                    x = len(nodos)-1
                elif(x == -2):
                    x = len(nodos)-2
            if(y >= len(nodos[0])):
                if(y == len(nodos[0])):
                    y = 0
                if(y == len(nodos[0]) + 1):
                    y = 1
            elif(y < 0):
                if(y == -1):
                    y = len(nodos[0])-1
                elif(y == -2):
                    y = len(nodos[0])-2
            
            #print("c: ", c.getX(), c.getY() , "\tpos: " , x , y)
            if(nodos[x][y].getTipo() != 'x'):
                c.addArrestas([x,y])

def equalNode(casa, c):
    if(casa.getX() == c.getX()):
        if(casa.getY() == c.getY()):
            return True
    return False

def caminhamentoLargura(casaInicial, casaFinal):
    casaInicial.setDist(0)
    queue = []
    queue.append(casaInicial)
    
    while (len(queue) > 0):
        casaAtual = queue[0]
        queue.pop(0)

        geraArrestas(casaAtual)
        #print(casaAtual.getArrestas() , "\n\n")
        
        for arrestaPar in casaAtual.getArrestas():
            tempPar = nodos[arrestaPar[0]][arrestaPar[1]]
            
            if tempPar.getCor() == "BRANCO" :
                tempPar.setDist(casaAtual.getDist() + 1)
                if equalNode(casaFinal, casaAtual):
                    return
                tempPar.setCor("CINZA"); 
                queue.append(tempPar)
                #print(arresta)
                    
        casaAtual.setCor("AMARELO")
        
    if (len(queue) == 0):
        print("Explorei todos nodos possiveis")




for test in testCases: #550
    ini = time.time()
    horse = []
    exit = []
    nodos = []
    arestas = set()
    file_name = ".\Casos\caso" + str(test) + ".txt"
    lines = open(file_name,'r')
    geraListaCasas(lines)

    
    med = time.time()
    print("PARA O TESTE " , str(test))
  

    caminhamentoLargura(nodos[horse[0]][horse[1]], nodos[exit[0]][exit[1]])
    final = time.time()
    print("resultado " + str(nodos[exit[0]][exit[1]].getDist()))
    print("tempo do caminhamento: " , (final-med))
    print("Tempo total: " , (final-ini), "\n\n")