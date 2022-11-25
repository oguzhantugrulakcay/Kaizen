using System;
using System.Text.Json;

class Program
{
    public class BoundingPoly
    {
        public List<Vertex> vertices { get; set; }
    }

    public class Response
    {
        public string locale { get; set; }
        public string description { get; set; }
        public BoundingPoly boundingPoly { get; set; }
    }

    public class Vertex
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class BillLine
    {
        public int No { get; set; }
        public string Text { get; set; }
        public int yValue { get; set; }
    }

    static void Main()
    {
        List<Response> response = new List<Response>();

        using (StreamReader r = new StreamReader("response.json"))
        {
            string json = r.ReadToEnd();
            response = JsonSerializer.Deserialize<List<Response>>(json);

        }

        var bill =new List<BillLine>();

        var firsLine = new BillLine
        {
            No = 1,
            Text = response[1].description,
            yValue= response[1].boundingPoly.vertices[0].y
        };
        bill.Add(firsLine);

        for (int i = 2; i < response.Count(); i++)
        {
            var lastLine = bill.Last();
            var y = response[i].boundingPoly.vertices[0].y;
            var findLine= bill
            .Where(c => Math.Abs( c.yValue - y )< 10)
            .Select(c => c.No)
            .FirstOrDefault();
            if (findLine == 0)
            {
                var justLine = new BillLine
                {
                    No = lastLine.No + 1,
                    Text = response[i].description,
                    yValue = response[i].boundingPoly.vertices[0].y
                };
                bill.Add(justLine);
            }
            else
            {
                var selectedLine = bill
                    .Where(b => b.No == findLine)
                    .FirstOrDefault();
                selectedLine.Text = selectedLine.Text + " "+ response[i].description;
            }

        }
        foreach (var line in bill)
        {
            Console.WriteLine(line.No + " " + line.Text);
        }
        Console.ReadLine();
        
    }
}