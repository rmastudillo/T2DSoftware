public static class GeneradorNumerosAleatorios
{ 
    private const int RandomSeed = 0; 
    private static Random rng = new Random ( RandomSeed ) ; 
    public static double Generar () => rng . Next () ;
}