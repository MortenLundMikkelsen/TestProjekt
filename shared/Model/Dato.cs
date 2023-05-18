namespace shared.Model;

public class Dato {
    public int DatoId { get; set; }
    public DateTime dato { get; set; }

    public Dato(DateTime Dato)
    {
        this.dato = Dato;

    }

    public Dato()
    {

    }
}