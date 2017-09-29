using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace jinyinmao.Signature.Service.Tests
{
    [TestClass]
    public class FDDServiceTests
    {
        [TestMethod]
        public void ClearContractFileTest()
        {
        }

        [TestMethod]
        public void FDDServiceTest()
        {
            try
            {
                FDDService service = new FDDService();

                Assert.IsNotNull(service);
            }
            catch (Exception ex)
            {
                Assert.Fail("实例化FDDService出错:" + ex.Message);
            }
        }

        [TestMethod]
        public void GenerationSigleOrderContractAsyncTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GenerationSigleProductContractAsyncTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetProductOrderInfoAsyncTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetUserIdByOrderIdTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SendTirifalAsyncTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void UploadContractTemplateAsyncTest()
        {
            throw new NotImplementedException();
        }
    }
}