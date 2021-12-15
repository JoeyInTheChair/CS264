using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;

namespace CS264Assignment5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            bool cont = true;
            string inp = "";
            string svg = "";
            List<Shape> canvas = new List<Shape>();
            Caretaker undoRedo = new Caretaker();
            WriteLine("List of commands: ");
            WriteLine("->A<shape>     Add shape to canvas");
            WriteLine("->U     Undo last operation");
            WriteLine("->R     Redo last operation");
            WriteLine("->C     Clear canvas");
            WriteLine("->Q     Quit");
            WriteLine("\n------------------------------------------------------------------------------------------------\n");
            while (cont)
            {
                WriteLine("Enter Command: ");
                Shape shape;
                Factory createShape;
                inp = ReadLine();
                string shapeInp = "";
                if (inp.Equals("A") || inp.Equals("a"))
                {
                    WriteLine("Forgot to type in shape");
                    WriteLine("List of valid shapes: \n ->Rectangle \n ->Circle \n ->Ellispe \n ->Line \n ->Polyline \n ->Polygon \n ->Path \n ->Text");
                    inp = "";
                }
                else if (inp.Contains("A") || inp.Contains("a"))
                {
                    shapeInp = inp.Substring(2);
                    inp = inp.Substring(0, 1).ToUpper();
                    shapeInp = shapeInp.Substring(0, 1).ToUpper() + shapeInp.Substring(1).ToLower();
                }
                else { inp = inp.ToUpper(); }
                switch (inp)
                {
                    case "A":
                        createShape = new Factory(shapeInp);
                        shape = createShape.shape;
                        WriteLine("Would you like to style " + shape.name + "?\n(Answer with Y/N)");
                        string ans = ReadLine().ToUpper();
                        if(ans.Equals("Y"))
                        {
                            WriteLine("Enter Stroke Width Value: ");
                            int strokeWidth = Convert.ToInt32(ReadLine());
                            WriteLine("Enter Stroke Colour Value: ");
                            string stroke = ReadLine();
                            WriteLine("Enter Fill Colour value: ");
                            string fill = ReadLine();
                            StyleShape style = new StyleShape(shape, strokeWidth, stroke, fill);
                            shape = style.shape;
                            WriteLine("New style for " + shape.name + ": (Stroke Width=" + shape.strokeWidth + ", Stroke=" + shape.stroke + ", Fill=" + shape.fill + ")");
                        }
                        canvas.Add(shape);
                        break;
                    case "U":
                        if (canvas.Count == 0) { WriteLine("There are no more shapes in the canvas"); }
                        else { undoShapeCanvas(canvas, undoRedo); }
                        break;
                    case "R":
                        if (undoRedo.getSize() == 0) { WriteLine("There are no more shapes to redo"); }
                        else { redoShapeCanvas(canvas, undoRedo); }
                        break;
                    case "C":
                        canvas.Clear();
                        undoRedo = new Caretaker();
                        WriteLine("canvas cleared!");
                        break;
                    case "Q":
                        svg = createSVG(canvas);
                        WriteLine("File canvas.svg has been succesfully saved!");
                        File.WriteAllText("canvas.svg", svg);
                        cont = false;
                        break;
                    default:
                        WriteLine("Invalid Input");
                        break;
                }
                WriteLine();
                inp = "";
            }
        }
        public static string createSVG(List<Shape> shape)
        {
            string start = "<svg width=\"1000\" height=\"1000\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">";
            string middle = "";
            string end = "</svg>";
            foreach (Shape s in shape) { middle += "\n" + s.getCode(); }
            return start + middle + "\n" + end;
        }
        static void undoShapeCanvas(List<Shape> canvas, Caretaker undoRedo)
        {
            Memento state = new Memento();
            Shape shape = canvas[canvas.Count - 1];
            state.setShape(shape);
            canvas.Remove(shape);
            undoRedo.addState(state);
            WriteLine(shape.name + " has been removed from canvas");
        }
        static void redoShapeCanvas(List<Shape> canvas, Caretaker undoRedo)
        {
            Memento state = undoRedo.getState();
            Shape shape = state.getShape();
            canvas.Add(shape);
            WriteLine(shape.name + " has been added back to the canvas");
        }
    }
        class StyleShape
        {
            public Shape shape { get; set; }
            public StyleShape(Shape shape, int strokeWidth, string stroke, string fill) 
            {
                this.shape = shape;
                this.shape.strokeWidth = strokeWidth;
                this.shape.stroke = stroke;
                this.shape.fill = fill;
            }
        }
        class Factory
        {
            public Shape shape { set; get; }
            public Factory(string s)
            {
                switch (s)
                {
                    case "Rectangle":
                        Rectangle rect = new Rectangle(randomNumGen(), randomNumGen(), randomNumGen(), randomNumGen());
                        this.shape = rect;
                        WriteLine(this.shape.name + " (X=" + rect.x + ",Y=" + rect.y + ",W=" + rect.w + ",H=" + rect.h + ") added to canvas.");
                        break;
                    case "Circle":
                        Circle circ = new Circle(randomNumGen(), randomNumGen(), randomNumGen());
                        this.shape = circ;
                        WriteLine(this.shape.name + " (X=" + circ.cx + ",Y=" + circ.cy + ",R=" + circ.rad + ") added to canvas.");
                        break;
                    case "Ellipse":
                        Ellipse ell = new Ellipse(randomNumGen(), randomNumGen(), randomNumGen(), randomNumGen());
                        this.shape = ell;
                        WriteLine(this.shape.name + " (RX=" + ell.rx + ",RY=" + ell.rx + ",CX=" + ell.cy + ",CY= " + ell.cy + ") added to canvas.");
                        break;
                    case "Line":
                        Line li = new Line(randomNumGen(), randomNumGen(), randomNumGen(), randomNumGen());
                        this.shape = li;
                        WriteLine(this.shape.name + " (X1=" + li.x1 + ",Y1=" + li.y1 + ",X2=" + li.x2 + ",Y2=" + li.x2 + ") added to canvas.");
                        break;
                    case "Polyline":
                        Polyline line = new Polyline(setPoints(s));
                        this.shape = line;
                        WriteLine(this.shape.name + " (Co-Ordinates=" + line.coord + ") added to canvas.");
                        break;
                    case "Polygon":
                        Polygon gon = new Polygon(setPoints(s));
                        this.shape = gon;
                        WriteLine(this.shape.name + " (Co-Ordinates=" + gon.coord + ") added to canvas.");
                        break;
                    case "Path":
                        Path p = new Path(setPoints(s));
                        this.shape = p;
                        WriteLine(this.shape.name + " (Co-Ordinates=" + p.coord + ") added to canvas.");
                        break;
                }
            }
            public int randomNumGen()
            {
                var rand = new Random();
                int r = rand.Next(10, 301);
                return r;
            }
            public string setPoints(string c)
            {
                var rand = new Random();
                string points = "";
                string[] pathPoints = { "L", "V", "H", "C", "S", "T", "A" };
                if (c.Equals("Path"))
                {
                    for (int i = 0; i < rand.Next(5, 16); i++)
                    {
                        string val = pathPoints[rand.Next(0, pathPoints.Length - 1)];
                        if (val == "H" || val == "V") { points += val + " " + randomNumGen() + " "; }
                        else if (val == "M" || val == "L" || val == "T") { points += val + " " + randomNumGen() + " " + randomNumGen() + " "; }
                        else if (val == "S" || val == "Q") { points += val + " " + randomNumGen() + " " + randomNumGen() + " " + randomNumGen() + " " + randomNumGen() + " "; }
                        else if (val == "C") { points += val + " " + randomNumGen() + " " + randomNumGen() + " " + randomNumGen() + " " + randomNumGen() + " " + randomNumGen() + " " + randomNumGen() + " "; }
                    }
                    points += "Z ";
                }
                else
                {
                    for (int i = 0; i < rand.Next(5, 16); i++)
                    {
                        points += randomNumGen().ToString() + " ";
                    }
                }
                return points.Substring(0, points.Length - 1);
            }
        }
        abstract class Shape
        {
            public string name { get; set; }
            public int strokeWidth { get; set; }
            public string stroke { get; set; }
            public string fill { get; set; }
            public virtual string getCode() { return ""; }
        }
        class Rectangle : Shape
        {
            public int x { get; set; }
            public int y { get; set; }
            public int w { get; set; }
            public int h { get; set; }
            public Rectangle(int x, int y, int w, int h)
            {
                this.name = "Rectangle";
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
                this.stroke = "black";
                this.strokeWidth = 1;
                this.fill = "grey";
            }
            public override string getCode()
            {
                return "    <rect x=\"" + this.x + "\" y=\"" + this.y + "\" width=\"" + this.w + "\" height=\"" + this.h + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
            }
        }
        class Circle : Shape
        {
            public int cx { get; set; }
            public int cy { get; set; }
            public int rad { get; set; }
            public Circle(int x, int y, int r)
            {
                this.name = "Circle";
                this.cx = x;
                this.cy = y;
                this.rad = r;
                this.stroke = "black";
                this.strokeWidth = 1;
                this.fill = "grey";
            }
            public override string getCode()
            {
                return "    <circle cx=\"" + this.cx + "\" cy=\"" + this.cy + "\" r=\"" + this.rad + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
            }
        }
        class Ellipse : Shape
        {
            public int cx { get; set; }
            public int cy { get; set; }
            public int rx { get; set; }
            public int ry { get; set; }
            public Ellipse(int x, int y, int r1, int r2)
            {
                this.name = "Ellipse";
                this.cx = x;
                this.cy = y;
                this.rx = r1;
                this.ry = 250;
                this.strokeWidth = 1;
                this.stroke = "black";
                this.fill = "grey";
            }
            public override string getCode()
            {
                return "    <ellipse cx=\"" + this.cx + "\" cy=\"" + this.cy + "\" rx=\"" + this.rx + "\" ry=\"" + this.ry + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
            }
        }
        class Line : Shape
        {
            public int x1 { get; set; }
            public int y1 { get; set; }
            public int x2 { get; set; }
            public int y2 { get; set; }
            public Line(int x, int y, int a, int b)
            {
                this.name = "Line";
                this.x1 = x;
                this.y1 = y;
                this.x2 = a;
                this.y2 = b;
                this.strokeWidth = 1;
                this.stroke = "black";
                this.fill = "grey";
            }
            public override string getCode()
            {
                return "    <line x1=\"" + this.x1 + "\" y1=\"" + this.y1 + "\" x2=\"" + this.x2 + "\" y2=\"" + this.y2 + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
            }
        }
        class Polyline : Shape
        {
            public string coord { get; set; }
            public Polyline(string s)
            {
                this.name = "Polyline";
                this.coord = s;
                this.strokeWidth = 1;
                this.stroke = "black";
                this.fill = "grey";
            }
            public override string getCode()
            {
                return "    <polyline fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" points=\"" + this.coord + "\"/>";
            }
        }
        class Polygon : Shape
        {
            public string coord { get; set; }
            public Polygon(string s)
            {
                this.name = "Polygon";
                this.coord = s;
                this.strokeWidth = 1;
                this.stroke = "black";
                this.fill = "grey";
            }
            public override string getCode()
            {
                return "    <polygon fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" points=\"" + this.coord + "\"/>";
            }
        }
        class Path : Shape
        {
            public string coord { get; set; }
            public Path(string s)
            {
                this.name = "Path";
                this.coord = s;
                this.strokeWidth = 1;
                this.stroke = "black";
                this.fill = "grey";
            }
            public override string getCode()
            {
                return "    <path fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" d=\"" + this.coord + "\"/>";
            }
        }
        class Memento
        {
            Shape shape;
            public Shape getShape()
            {
                return this.shape;
            }
            public void setShape(Shape s)
            {
                this.shape = s;
            }
        }
        class Caretaker
        {
            List<Memento> undoRedo = new List<Memento>();
            Memento state = new Memento();
            public Memento getState()
            {
                this.state = this.undoRedo[undoRedo.Count - 1];
                this.undoRedo.RemoveAt(undoRedo.Count - 1);
                return this.state;
            }
            public void addState(Memento m)
            {
                this.state = m;
                this.undoRedo.Add(state);
            }
            public int getSize() { return this.undoRedo.Count; }
        }
}