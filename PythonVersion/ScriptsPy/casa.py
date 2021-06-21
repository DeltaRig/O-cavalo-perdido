class Casa:

    _pos = []
    _tipo = ""
    _dist = 99999
    _cor = "BRANCO"


    def __init__(self, pos, tipo):
        self._pos = pos
        self._tipo = tipo

    def getX(self):
        return self._pos[0]

    def getY(self):
        return self._pos[1]

    def getPos(self):
        return self._pos

    def getTipo(self):
        return self._tipo

    def getCor(self):
        return self._cor
    
    def getDist(self):
        return self._dist

    def setCor(self, cor):
        self._cor = cor

    def setDist(self, dist):
        self._dist = dist

    def __str__(self):
        return str(self._pos) + " " + str(self._tipo)
    