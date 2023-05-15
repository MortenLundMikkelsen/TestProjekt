using System;
using shared.Model;
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
        PN pn = new PN(DateTime.Now, DateTime.Now.AddDays(3), 5 , lm);
        Assert.AreEqual(4, pn.antalDage());

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


    [TestMethod]
    public void DoegnDosisTest()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        PN pn = service.GetPNs().First();
        pn.givDosis(new Dato(DateTime(2021,1,5);

    }


}