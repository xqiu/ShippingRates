using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using ShippingRates.ShippingProviders;

namespace ShippingRates.Tests.ShippingProviders
{
    [TestFixture]
    public class UPSRates
    {
        private Address InternationalAddress2;
        private Package Package2;
        private readonly Address DomesticAddress1;
        private readonly Address DomesticAddress2;
        private readonly Address InternationalAddress1;
        private readonly Package Package1;
        private readonly Package Package1SignatureRequired;
        private readonly string UPSLicenseNumber;
        private readonly string UPSPassword;
        private readonly string UPSUserId;

        public UPSRates()
        {
            DomesticAddress1 = new Address("278 Buckley Jones Road", "", "", "Cleveland", "MS", "38732", "US");
            DomesticAddress2 = new Address("One Microsoft Way", "", "", "Redmond", "WA", "98052", "US");
            InternationalAddress1 = new Address("Porscheplatz 1", "", "", "70435 Stuttgart", "", "", "DE");
            InternationalAddress2 = new Address("80-100 Victoria St", "", "", "London SW1E 5JL", "", "", "GB");

            Package1 = new Package(4, 4, 4, 5, 0);
            Package2 = new Package(6, 6, 6, 5, 100);
            Package1SignatureRequired = new Package(4, 4, 4, 5, 0, null, true);

            var config = ConfigHelper.GetApplicationConfiguration(TestContext.CurrentContext.TestDirectory);

            UPSUserId = config.UPSUserId;
            UPSPassword = config.UPSPassword;
            UPSLicenseNumber = config.UPSLicenseNumber;
        }

        [Test]
        public void UPS_Domestic_Returns_Rates_When_Using_International_Addresses_For_Single_Service()
        {
            var rateManager = new RateManager();
            rateManager.AddProvider(new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword, "UPS Worldwide Express"));

            var response = rateManager.GetRates(DomesticAddress1, InternationalAddress1, Package1);

            Debug.WriteLine($"Rates returned: {(response.Rates.Any() ? response.Rates.Count.ToString() : "0")}");

            Assert.NotNull(response);
            Assert.IsNotEmpty(response.Rates);
            Assert.IsEmpty(response.Errors);

            foreach (var rate in response.Rates)
            {
                Assert.NotNull(rate);
                Assert.True(rate.TotalCharges > 0);

                Debug.WriteLine(rate.Name + ": " + rate.TotalCharges);
            }
        }

        [Test]
        public void UPS_Returns_Multiple_Rates_When_Using_Valid_Addresses_For_All_Services()
        {
            var rateManager = new RateManager();
            rateManager.AddProvider(new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword));

            var response = rateManager.GetRates(DomesticAddress1, DomesticAddress2, Package1);

            Debug.WriteLine($"Rates returned: {(response.Rates.Any() ? response.Rates.Count.ToString() : "0")}");

            Assert.NotNull(response);
            Assert.IsNotEmpty(response.Rates);
            Assert.IsEmpty(response.Errors);

            foreach (var rate in response.Rates)
            {
                Assert.NotNull(rate);
                Assert.True(rate.TotalCharges > 0);

                Debug.WriteLine(rate.Name + ": " + rate.TotalCharges);
            }
        }

        [Test]
        public void UPS_Returns_Multiple_Rates_When_Using_Valid_Addresses_For_All_Services_And_Multple_Packages()
        {
            var rateManager = new RateManager();
            rateManager.AddProvider(new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword));

            var response = rateManager.GetRates(DomesticAddress1, DomesticAddress2, Package1);

            Debug.WriteLine($"Rates returned: {(response.Rates.Any() ? response.Rates.Count.ToString() : "0")}");

            Assert.NotNull(response);
            Assert.IsNotEmpty(response.Rates);
            Assert.IsEmpty(response.Errors);

            foreach (var rate in response.Rates)
            {
                Assert.NotNull(rate);
                Assert.True(rate.TotalCharges > 0);

                Debug.WriteLine(rate.Name + ": " + rate.TotalCharges);
            }
        }

        [Test]
        public void UPS_Returns_Rates_When_Using_International_Origin_And_Destination_Addresses_For_All_Services()
        {
            var rateManager = new RateManager();
            rateManager.AddProvider(new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword));

            var response = rateManager.GetRates(InternationalAddress2, InternationalAddress1, Package1);

            Debug.WriteLine($"Rates returned: {(response.Rates.Any() ? response.Rates.Count.ToString() : "0")}");

            Assert.NotNull(response);
            Assert.IsNotEmpty(response.Rates);
            Assert.IsEmpty(response.Errors);

            foreach (var rate in response.Rates)
            {
                Assert.NotNull(rate);
                Assert.True(rate.TotalCharges > 0);

                Debug.WriteLine(rate.Name + ": " + rate.TotalCharges);
            }
        }

        [Test]
        public void UPS_Returns_Rates_When_Using_International_Destination_Addresses_And_RetailRates_For_All_Services()
        {
            var rateManager = new RateManager();
            var provider = new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword)
            {
                UseRetailRates = true
            };

            rateManager.AddProvider(provider);

            var response = rateManager.GetRates(DomesticAddress1, InternationalAddress1, Package1);

            Debug.WriteLine($"Rates returned: {(response.Rates.Any() ? response.Rates.Count.ToString() : "0")}");

            Assert.NotNull(response);
            Assert.IsNotEmpty(response.Rates);
            Assert.IsEmpty(response.Errors);

            foreach (var rate in response.Rates)
            {
                Assert.NotNull(rate);
                Assert.True(rate.TotalCharges > 0);

                Debug.WriteLine(rate.Name + ": " + rate.TotalCharges);
            }
        }

        [Test]
        public void UPS_Returns_Rates_When_Using_International_Destination_Addresses_For_All_Services()
        {
            var rateManager = new RateManager();
            rateManager.AddProvider(new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword));

            var response = rateManager.GetRates(DomesticAddress1, InternationalAddress1, Package1);

            Debug.WriteLine($"Rates returned: {(response.Rates.Any() ? response.Rates.Count.ToString() : "0")}");

            Assert.NotNull(response);
            Assert.IsNotEmpty(response.Rates);
            Assert.IsEmpty(response.Errors);

            foreach (var rate in response.Rates)
            {
                Assert.NotNull(rate);
                Assert.True(rate.TotalCharges > 0);

                Debug.WriteLine(rate.Name + ": " + rate.TotalCharges);
            }
        }

        [Test]
        public void UPS_Returns_Single_Rate_When_Using_Domestic_Addresses_For_Single_Service()
        {
            var rateManager = new RateManager();
            rateManager.AddProvider(new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword, "UPS Ground"));

            var response = rateManager.GetRates(DomesticAddress1, DomesticAddress2, Package1);

            Debug.WriteLine($"Rates returned: {(response.Rates.Any() ? response.Rates.Count.ToString() : "0")}");

            Assert.NotNull(response);
            Assert.IsNotEmpty(response.Rates);
            Assert.IsEmpty(response.Errors);
            Assert.AreEqual(response.Rates.Count, 1);
            Assert.True(response.Rates.First().TotalCharges > 0);

            Debug.WriteLine(response.Rates.First().Name + ": " + response.Rates.First().TotalCharges);
        }

        [Test]
        public void CanGetUpsServiceCodes()
        {
            var provider = new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword);
            var serviceCodes = provider.GetServiceCodes();

            Assert.NotNull(serviceCodes);
            Assert.IsNotEmpty(serviceCodes);
        }

        [Test]
        public void Can_Get_Different_Rates_For_Signature_Required_Lookup()
        {
            var rateManager = new RateManager();
            rateManager.AddProvider(new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword, "UPS Ground"));

            var nonSignatureResponse = rateManager.GetRates(DomesticAddress1, DomesticAddress2, Package1);
            var signatureResponse = rateManager.GetRates(DomesticAddress1, DomesticAddress2, Package1SignatureRequired);

            Debug.WriteLine(string.Format("Rates returned: {0}", nonSignatureResponse.Rates.Any() ? nonSignatureResponse.Rates.Count.ToString() : "0"));

            Assert.NotNull(nonSignatureResponse);
            Assert.IsNotEmpty(nonSignatureResponse.Rates);
            Assert.IsEmpty(nonSignatureResponse.Errors);
            Assert.AreEqual(nonSignatureResponse.Rates.Count, 1);
            Assert.True(nonSignatureResponse.Rates.First().TotalCharges > 0);

            Debug.WriteLine(string.Format("Rates returned: {0}", signatureResponse.Rates.Any() ? signatureResponse.Rates.Count.ToString() : "0"));

            Assert.NotNull(signatureResponse);
            Assert.IsNotEmpty(signatureResponse.Rates);
            Assert.IsEmpty(signatureResponse.Errors);
            Assert.AreEqual(signatureResponse.Rates.Count, 1);
            Assert.True(signatureResponse.Rates.First().TotalCharges > 0);

            // Now compare prices
            foreach (var signatureRate in signatureResponse.Rates)
            {
                var nonSignatureRate = nonSignatureResponse.Rates.FirstOrDefault(x => x.Name == signatureRate.Name);

                if (nonSignatureRate != null)
                {
                    var signatureTotalCharges = signatureRate.TotalCharges;
                    var nonSignatureTotalCharges = nonSignatureRate.TotalCharges;
                    Assert.AreNotEqual(signatureTotalCharges, nonSignatureTotalCharges);
                }
            }
        }

        [Test]
        public async Task UPSSaturdayDelivery()
        {
            var from = new Address("Annapolis", "MD", "21401", "US");
            var to = new Address("Fitchburg", "WI", "53711", "US");
            var package = new Package(7, 7, 7, 6, 0);

            var today = DateTime.Now;
            var nextFriday = today.AddDays(12 - (int)today.DayOfWeek);

            var rateManager = new RateManager();
            rateManager.AddProvider(new UPSProvider(UPSLicenseNumber, UPSUserId, UPSPassword));

            var r = await rateManager.GetRatesAsync(from, to, package, new ShipmentOptions()
            {
                ShippingDate = nextFriday,
                SaturdayDelivery = true
            });
            var fedExRates = r.Rates.ToList();

            Assert.NotNull(r);
            Assert.True(fedExRates.Any());
            Assert.True(fedExRates.Any(r => r.Options.SaturdayDelivery));
        }
    }
}
