namespace ordination_test;

using Microsoft.EntityFrameworkCore;
using Service;
using Data;
using shared.Model;
using ordination_api;
using static shared.Util;

[TestClass]
public class ServiceTest
{
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
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }


    [TestMethod]
    public void OpretDagligSkæv()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        Dosis[] doser = new Dosis[] {
                new Dosis(CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(CreateTimeOnly(12, 40, 0), 1),
                new Dosis(CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(CreateTimeOnly(18, 45, 0), 3) };
           

        Assert.AreEqual(1, service.GetDagligSkæve().Count());
        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId, doser,DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligSkæve().Count());

    }

    [TestMethod]
    public void OpretPN()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(4, service.GetPNs().Count());
        service.OpretPN(patient.PatientId, lm.LaegemiddelId,1, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(5, service.GetPNs().Count());
    }


   
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), "Patient ej fundet")]
    public void ExceptionOpretPN()
    {
        // Arrange
        Patient patient = null;
        Laegemiddel lm = service.GetLaegemidler().First();

        int initialPNCount = service.GetPNs().Count();
        // Vi tillader i testen at patient må være null
        service.OpretPN(patient?.PatientId ?? 0, lm.LaegemiddelId, 1, DateTime.Now, DateTime.Now.AddDays(3));
       

        // Assert
        int updatedPNCount = service.GetPNs().Count();
        Assert.AreEqual(initialPNCount, updatedPNCount);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), "Patient ej fundet")]
    public void ExceptionOpretDagligFast()
    {
        // Arrange
        Patient patient = null;
        Laegemiddel lm = service.GetLaegemidler().First();

        int initialPNCount = service.GetDagligFaste().Count();
        // Vi tillader i testen at patient må være null
        service.OpretDagligFast(patient?.PatientId ?? 0, 2, 1, 0, lm.LaegemiddelId, 1, DateTime.Now, DateTime.Now.AddDays(3));


        // Assert
        int updatedPNCount = service.GetDagligFaste().Count();
        Assert.AreEqual(initialPNCount, updatedPNCount);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException), "Patient ej fundet")]
    public void ExceptionOpretDagligSkæv()
    {
        // Arrange
        Patient patient = null;
        Laegemiddel lm = service.GetLaegemidler().First();
        Dosis[] doser = new Dosis[] {
                new Dosis(CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(CreateTimeOnly(12, 40, 0), 1),
                new Dosis(CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(CreateTimeOnly(18, 45, 0), 3) };

        int initialPNCount = service.GetDagligSkæve().Count();
        // Vi tillader i testen at patient må være null
        service.OpretDagligSkaev(patient?.PatientId ?? 0, lm.LaegemiddelId, doser, DateTime.Now, DateTime.Now.AddDays(3));


        // Assert
        int updatedPNCount = service.GetDagligSkæve().Count();
        Assert.AreEqual(initialPNCount, updatedPNCount);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }


    [TestMethod]
    public void TestAnbefaletdosisPerDøgn()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(lm.enhedPrKgPrDoegnNormal * patient.vaegt, service.GetAnbefaletDosisPerDøgn(patient.PatientId, lm.LaegemiddelId));
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "Patient ej fundet")]
    public void ExceptionAnbefaletDosis()
    {
        Patient patient = null;
        Laegemiddel lm = service.GetLaegemidler().First();

        service.GetAnbefaletDosisPerDøgn(patient?.PatientId ?? 0, lm.LaegemiddelId);

    }

}