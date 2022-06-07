

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> square = x => x*x;
            Func<int, int, int> add = (int x, int y) =>
            {
                int temp = x + y;
                return temp;
            };

            Action<int> write = x => Console.WriteLine(x);

            write(square(add(3, 5)));

            var developers = new Employee[]
            {
                new Employee{Id = 1, Name = "Daniel" },
                new Employee{Id=2, Name ="Jackson"}
            };

            var sales = new List<Employee>()
            {
                new Employee{Id = 3, Name = "Chris"}
            };
            //var query = developers.Where(e => e.Name.Length >= 6).OrderByDescending(e => e.Name);
            var query = developers.Where(e => e.Name.Length > 5).OrderBy(e => e.Name).Select(e => e);
            foreach (var employee in query)
            {
                Console.WriteLine(employee.Name);
            }

        }
    }
}