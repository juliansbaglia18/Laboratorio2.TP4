using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entidades;

namespace UnitTest
{
    [TestClass]
    public class ListaInstanciada
    {
        [TestMethod]
        public void TESTListaPaquetesInstanciada()
        {
            Correo c1 = new Correo();
            if (c1.Paquetes.Equals(null))
            {
                Assert.Fail();
            }
        }
    }
}
