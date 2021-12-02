using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;

namespace CS264Assignment4
{
    class svgUndoRedo
    {
        static void Main(string[] args)
        {
            Console.Clear();
            bool run = true;
            string inp = "";
            //create canvas list to store inputted shape
            List<Shape> canvas = new List<Shape>();
            User user = new User();
            WriteLine("Type in \"H\" to see command list");
            while (run)
            {
                //take in user input
                inp = ReadLine();
                string shape = "";
                if (inp.Contains("A") || inp.Contains("a"))
                {
                    if(inp.Length >= 2)
                    {
                        shape = inp.Substring(2);
                        inp = inp.Substring(0, 1).ToUpper();
                        shape = shape.Substring(0, 1).ToUpper() + shape.Substring(1).ToLower();
                    }
                    else
                    {
                        WriteLine("You forgot to add a shape, ");
                        WriteLine("List of valid shapes: \n ->Rectangle \n ->Circle \n ->Ellipse \n ->Line \n ->Polyline \n ->Polygon \n ->Path\n");
                    }
                }
                else { inp = inp.ToUpper(); }
                //use switch statement to run through appropriate lines
                switch (inp)
                {
                    //H - Command List
                    case "H":
                        WriteLine("List of commands: ");
                        WriteLine("->H     See command list");
                        WriteLine("->A<shape>     Add shape to canvas");
                        WriteLine("->U     Undo last operation");
                        WriteLine("->R     Redo last operation");
                        WriteLine("->C     Clear canvas");
                        WriteLine("->Q     Quit");
                        break;
                    //A - adding shape
                    case "A":
                        user.Action(new addShape(shape, canvas));
                        currentCanvas(canvas);
                        break;
                    //U - undo shape input
                    case "U":
                        user.Undo();
                        currentCanvas(canvas);
                        break;
                    //R - redo shape into canvas
                    case "R":
                        user.Redo();
                        currentCanvas(canvas);
                        break;
                    //C - clear canvas
                    case "C":
                        user.Action(new clearCanvas(canvas));
                        break;
                    //Q - quit the loop and export canvas to an svg file
                    case "Q":
                        saveSVG(canvas);
                        run = false;
                        break;
                    //tell user their input is invalid
                    default:
                        WriteLine("Invalid Input\nType in \"H\" to see command list\n");
                        break;
                }
                inp = "";
            }
        }
        //method is used to save the canvas of shapes into a svg file
        static void saveSVG(List <Shape> canvas)
        {
            //first retrieve the svg format of the shapes
            string svg = getSVG(canvas);
            WriteLine("Your svg file: " + "\n" + svg);
            File.WriteAllText(@"./canvas.svg", svg);
            WriteLine("canvas.svg File has been created!!!");
        }
        //method returns a string of the list of shapes in a svg format
        static string getSVG(List <Shape> canvas)
        {
            string start = "<svg width=\"1000\" height=\"1000\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">";
            string middle = "";
            string end = "</svg>";
            foreach (Shape s in canvas)
            {
                if (s.name.Equals("Rectangle")) { middle += "\n" + s.getCode(); }
                else if (s.name.Equals("Circle")) { middle += "\n" + s.getCode(); }
                else if (s.name.Equals("Ellipse")) { middle += "\n" + s.getCode(); }
                else if (s.name.Equals("Line")) { middle += "\n" + s.getCode(); }
                else if (s.name.Equals("Polyline")) { middle += "\n" + s.getCode(); }
                else if (s.name.Equals("Polygon")) { middle += "\n" + s.getCode(); }
                else if (s.name.Equals("Path")) { middle += "\n" + s.getCode(); }
                else if (s.name.Equals("Text")) { middle += "\n" + s.getCode(); }
            }
            return start + middle + "\n" + end;
        }
        static void currentCanvas(List<Shape> canvas)
        {
            WriteLine("\n---------------------------------------------------------------------\nShapes in canvas currently: ");
            foreach (Shape s in canvas)
            {
                if (s.name.Equals("Rectangle")) { WriteLine(s.shapeInfo()); }
                else if (s.name.Equals("Circle")) { WriteLine(s.shapeInfo()); }
                else if (s.name.Equals("Ellipse")) { WriteLine(s.shapeInfo()); }
                else if (s.name.Equals("Line")) { WriteLine(s.shapeInfo()); }
                else if (s.name.Equals("Polyline")) { WriteLine(s.shapeInfo()); }
                else if (s.name.Equals("Polygon")) { WriteLine(s.shapeInfo()); }
                else if (s.name.Equals("Path")) { WriteLine(s.shapeInfo()); }
                else if (s.name.Equals("Text")) { WriteLine(s.shapeInfo()); }
            }
            WriteLine("---------------------------------------------------------------------\n");
        }
    }
    //invoker class
    class User
    {
        //keep track of all commands made by user
        public Stack<Command> currentComs;
        public Stack<Command> pastComs;

        //used to keep track so not out of bounds
        public int undoC { get => currentComs.Count; }
        public int redoC { get => pastComs.Count; }

        public User()
        {
            //reset console confirming new user 
            //by clearing stacks
            Reset();
        }
        public void Reset()
        {
            currentComs = new Stack<Command>();
            pastComs = new Stack<Command>();
        }
        //get actions made by user
        public void Action(Command c)
        {
            currentComs.Push(c);
            pastComs.Clear();
            Type t = c.GetType();
            if(t.Equals(typeof(addShape)))
            {
                c.Do();
            }
            if (t.Equals(typeof(clearCanvas)))
            {
                c.Do();
            }
        }
        public void Undo()
        {
            if(currentComs.Count > 0)
            {
                Command c = currentComs.Pop();
                c.Undo();
                pastComs.Push(c);
            }
            else
            {
                WriteLine("Unable to undo anymore shapes");
            }
        }
        public void Redo()
        {
            if(pastComs.Count > 0)
            {
                Command c = pastComs.Pop();
                c.Do();
                currentComs.Push(c);
            }
            else
            {
                WriteLine("Unable to redo anymore shapes");
            }
        }

    }
    abstract class Command
    {
        public abstract void Do();
        public abstract void Undo();
    }
    class addShape : Command
    {
        public String temp;
        public Shape shape;
        public List<Shape> canvas;
        public addShape(string t, List<Shape> c)
        {
            this.temp = t;
            this.canvas = c;
        }
        public override void Do()
        {
            switch (this.temp)
            {
                case "Rectangle":
                    Rectangle rect = new Rectangle(randomNumGen(), randomNumGen(), randomNumGen(), randomNumGen());
                    this.shape = rect;
                    this.shape.fill = setFillColour();
                    canvas.Add(shape);
                    WriteLine(shape.name + " (X=" + rect.x + ",Y=" + rect.y + ",W=" + rect.w + ",H=" + rect.h + ") added to canvas.");
                    break;
                case "Circle":
                    Circle circ = new Circle(randomNumGen(), randomNumGen(), randomNumGen());
                    this.shape = circ;
                    this.shape.fill = setFillColour();
                    canvas.Add(shape);
                    WriteLine(shape.name + " (X=" + circ.cx + ",Y=" + circ.cy + ",R=" + circ.rad + ") added to canvas.");
                    break;
                case "Ellipse":
                    Ellipse ell = new Ellipse(randomNumGen(), randomNumGen(), randomNumGen(), randomNumGen());
                    this.shape = ell;
                    this.shape.fill = setFillColour();
                    canvas.Add(shape);
                    WriteLine(shape.name + " (RX=" + ell.rx + ",RY=" + ell.rx + ",CX=" + ell.cy + ",CY= " + ell.cy + ") added to canvas.");
                    break;
                case "Line":
                    Line li = new Line(randomNumGen(), randomNumGen(), randomNumGen(), randomNumGen());
                    this.shape = li;
                    this.shape.fill = setFillColour();
                    canvas.Add(shape);
                    WriteLine(shape.name + " (X1=" + li.x1 + ",Y1=" + li.y1 + ",X2=" + li.x2 + ",Y2=" + li.x2 + ") added to canvas.");
                    break;
                case "Polyline":
                    Polyline line = new Polyline(setPoints(this.temp));
                    this.shape = line;
                    this.shape.fill = setFillColour();
                    canvas.Add(shape);
                    WriteLine(shape.name + " (Co-Ordinates=" + line.coord + ") added to canvas.");
                    break;
                case "Polygon":
                    Polygon gon = new Polygon(setPoints(this.temp));
                    this.shape = gon;
                    this.shape.fill = setFillColour();
                    canvas.Add(shape);
                    WriteLine(shape.name + " (Co-Ordinates=" + gon.coord + ") added to canvas.");
                    break;
                case "Path":
                    Path p = new Path(setPoints(this.temp));
                    this.shape = p;
                    this.shape.fill = setFillColour();
                    canvas.Add(shape);
                    WriteLine(shape.name + " (Co-Ordinates=" + p.coord + ") added to canvas.");
                    break;
                default:
                    WriteLine("Invalid shape input");
                    WriteLine("List of valid shapes: \n ->Rectangle \n ->Circle \n ->Ellipse \n ->Line \n ->Polyline \n ->Polygon \n ->Path");
                    break;
            }
        }
        public override void Undo()
        {
            WriteLine(shape.name + " has been removed from canvas!");
            this.canvas.Remove(this.shape);
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
            string[] pathPoints = {"L", "V", "H", "C", "S", "T", "A", "Z" };
            if(c.Equals("Path"))
            {
                points += "M ";
                for(int i = 0; i < rand.Next(5, 16); i++)
                {
                    int val = rand.Next(0, 2);
                    if(val == 0)
                    {
                        points += randomNumGen().ToString() + " ";
                    }
                    else
                    {
                        points += pathPoints[rand.Next(0, pathPoints.Length)] + " ";
                    }
                }
            }
            else
            {
                for(int i = 0; i < rand.Next(5, 16); i++)
                {
                    points += randomNumGen().ToString() + " ";
                }
            }
            return points.Substring(0, points.Length-1);
        }
        public string setFillColour()
        {
            string colour = "";
            var rand = new Random();
            switch(rand.Next(1,11))
            {
                case 1: colour = "red";
                    break;
                case 2: colour = "blue";
                    break;
                case 3: colour = "green";
                    break;
                case 4: colour = "blueviolet";
                    break;
                case 5: colour = "cadetblue";
                    break;
                case 6: colour = "aquamarine";
                    break;
                case 7: colour = "coral";
                    break;
                case 8: colour = "crimson";
                    break;
                case 9: colour = "chartreuse";
                    break;
                case 10: colour = "burlywood";
                    break;
            }
            return colour;
        }
    }
    class clearCanvas : Command
    {
        public List<Shape> canvas;
        public clearCanvas(List<Shape> c)
        {
            this.canvas = c;
        }
        public override void Do()
        {
            this.canvas.Clear();
            WriteLine("Canvas has been cleared");
        }
        public override void Undo() { }
    }
    //create parent class Shape
    //inheriting Shape is rectangle, circle, ellipse, line, polyline, polygon, path & text
    //each class has their own respective variables with getters and setters
    //getCode() method return the svg line of that shape
    abstract class Shape
    {
        //shape class variables
        public string name { get; set; }
        public int strokeWidth { get; set; }
        public string stroke { get; set; }
        public string fill { get; set; }
        public virtual string getCode() { return ""; }
        public virtual string shapeInfo() { return ""; }
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
            return "    <rect x=\"" + this.x + "\" y=\"" + this.y + "\" width=\"" + this.w + "\" height=\"" + this.h + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" fill-opacity=\"0.5\"/>";
        }
        public override string shapeInfo()
        {
            return this.name + " (X=" + this.x + ",Y=" + this.y + ",W=" + this.w + ",H=" + this.h + ")";
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
            return "    <circle cx=\"" + this.cx + "\" cy=\"" + this.cy + "\" r=\"" + this.rad + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" fill-opacity=\"0.5\"/>";
        }
        public override string shapeInfo()
        {
            return this.name + " (X=" + this.cx + ",Y=" + this.cy + ",R=" + this.rad + ")";
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
            return "    <ellipse cx=\"" + this.cx + "\" cy=\"" + this.cy + "\" rx=\"" + this.rx + "\" ry=\"" + this.ry + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" fill-opacity=\"0.5\"/>";
        }
        public override string shapeInfo()
        {
            return this.name + " (RX=" + this.rx + ",RY=" + this.rx + ",CX=" + this.cy + ",CY= " + this.cy + ")";
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
            return "    <line x1=\"" + this.x1 + "\" y1=\"" + this.y1 + "\" x2=\"" + this.x2 + "\" y2=\"" + this.y2 + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" fill-opacity=\"0.5\"/>";
        }
        public override string shapeInfo()
        {
            return this.name + " (X1=" + this.x1 + ",Y1=" + this.y1 + ",X2=" + this.x2 + ",Y2=" + this.x2 + ")";
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
            return "    <polyline fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" points=\"" + this.coord + "\" fill-opacity=\"0.5\"/>";
        }
        public override string shapeInfo()
        {
            return this.name + " (points=" + this.coord + ")";
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
            return "    <polygon fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" points=\"" + this.coord + "\" fill-opacity=\"0.5\"/>";
        }
        public override string shapeInfo()
        {
            return this.name + " (points=" + this.coord + ")";
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
            return "    <path fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" d=\"" + this.coord + "\" fill-opacity=\"0.5\"/>";
        }
        public override string shapeInfo()
        {
            return this.name + " (d=" + this.coord + ")";
        }
    }
}