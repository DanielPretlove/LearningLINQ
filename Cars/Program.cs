using System.Xml.Linq;

namespace Cars
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateXml();
            QueryXml();
        }

        private static void QueryXml()
        {
            XNamespace ns = "http://pluralsight.com/cars/2016";
            XNamespace ex = "http://pluralsight.com/cars/2016/ex";
            var document = XDocument.Load("fuel.xml");

            var query = from element in document.Element(ns + "Cars").Elements(ex + "Car") 
                                                                ?? Enumerable.Empty<XElement>()
                        where element.Attribute("Manufacturer").Value == "BMW" 
                        select element.Attribute("Name").Value;

            foreach(var name in query)
            {
                Console.WriteLine(name);
            }
        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");

            XNamespace ns = "http://pluralsight.com/cars/2016";
            XNamespace ex = "http://pluralsight.com/cars/2016/ex";
            var document = new XDocument();
            var cars = new XElement(ns + "Car",
                    from record in records
                    select new XElement(ex + "Car",
                            new XAttribute("Name", record.Name),
                            new XAttribute("Combined", record.Combined),
                            new XAttribute("Manufacturer", record.Manufacturer)));
            document.Add(cars);
            document.Save("fuel.xml");

            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));
        }

        public class CarStatistics
        {
            public CarStatistics()
            {
                Max = Int32.MaxValue;
                Min = Int32.MinValue;

            }

            public CarStatistics Accumlate(Car car)
            {
                Count += 1;
                Total += car.Combined;
                Max = Math.Max(Max, car.Combined);
                Min = Math.Min(Min, car.Combined);
                return this;
            }

            public CarStatistics Compute()
            {
                Average = Total / Count;
                return this;
            }

            public int Max { get; set; }
            public int Min { get; set; }
            public int Total { get; set; }
            public int Count { get; set; }
            public double Average { get; set; }
        }

        private static List<Car> ProcessCars(string path)
        {
            var query = File.ReadAllLines(path)
                        .Skip(1)
                        .Where(l => l.Length > 1).ToCar();
            return query.ToList();
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path).Where(l => l.Length > 1).Select(l =>
            {
                var comlumns = l.Split(",");
                return new Manufacturer
                {
                    Name = comlumns[0],
                    Headquaters = comlumns[1],
                    Year = int.Parse(comlumns[2])
                };
            });
            return query.ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {

            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new Car
                {

                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Combined = int.Parse(columns[6]),
                    Highway = int.Parse(columns[7])
                };
            }
     
        }
    }
}