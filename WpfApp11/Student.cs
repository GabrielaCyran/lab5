using System;



public class Student 

{
	public string imie { get; set; }
	public string nazwisko { get; set; }
	public int NrIndeksu { get; set; }
	public string Wydzial { get; set; }




	public Student(string imie, string nazwisko, int nrindeksu, string wydzial)
	{
		this.imie = imie;
		this.nazwisko = nazwisko;
		this.NrIndeksu = nrindeksu;
		this.Wydzial = wydzial;
		
	}

	public Student() : this("", "", 0, "")
	{

	}
}
