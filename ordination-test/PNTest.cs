using System;
using shared.Model;
using ordination_api;
namespace ordination_test;

using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
using static shared.Util;

[TestClass]
public class DatoTest {

    private DataService service;


    [TestInitialize]
    public void SetupBeforeEachTest()
    {
    var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
    optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
    var context = new OrdinationContext(optionsBuilder.Options);
    service = new DataService(context);
    service.SeedData();
    }

    [TestMethod]
    public void antalDageTest()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        // TC1
        PN pn = new PN(DateTime.Parse("2023,12,05"), DateTime.Parse("2023,12,12"), 5 , lm);
        Assert.AreEqual(8, pn.antalDage());

        //TC2
        PN pn2 = new PN(DateTime.Parse("2023,12,05"), DateTime.Parse("2024,03,01"), 5, lm);
        Assert.AreEqual(88, pn2.antalDage());

        //TC3
        PN pn3 = new PN(DateTime.Parse("2023,12,05"), DateTime.Parse("2024,01,05"), 5, lm);
        Assert.AreEqual(32, pn3.antalDage());


        //TC4
        PN pn4 = new PN(DateTime.Parse("2023,12,05"), DateTime.Parse("2023,12,05"), 5, lm);
        Assert.AreEqual(1, pn4.antalDage());

        

    }


    [TestMethod]
    [ExpectedException(typeof(Exception), "Antal dage må ikke være negativ")]
    public void AntalDageNegativTest()
    {
        // Arrange
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        PN pn = new PN(DateTime.Now, DateTime.Now.AddDays(-3), 5, lm);
        pn.antalDage();

    }


    // Muligvis ikke korrekt implementeret - Testen fungerer, men nok ikke korrekt.
    [TestMethod]
    public void DoegnDosisTest()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        PN pn = service.GetPNs().First();
        DateTime dato1 = new DateTime(2021,01,01);
        DateTime dato2 = new DateTime(2021,01,12);
        pn.givDosis(dato1);
        pn.givDosis(dato2);
        Assert.AreEqual(20.5, pn.doegnDosis());
    }

    

}