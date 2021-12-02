using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;

namespace CS264Assignment3
{
    class svgundoredo
    {
        static void Main(string[] args)
        {
            Console.Clear();
            bool run = true;
            string inp = "";
            string svg = "";
            //create canvas list to store inputted shape
            List<Shape> canvas = new List<Shape>();
            //create caretake variable for undo and redo
            Caretaker undoRedo = new Caretaker();
            WriteLine("List of commands: ");
            WriteLine("->A<shape>     Add shape to canvas");
            WriteLine("->A<shape>(boundaries)     Add shape to canvas");
            WriteLine("->S     Apply styles to shapes");
            WriteLine("->V     Print current svg ");
            WriteLine("->T     Add text to canvas");
            WriteLine("->U     Undo last operation");
            WriteLine("->R     Redo last operation");
            WriteLine("->C     Clear canvas");
            WriteLine("->Q     Quit");
            while (run)
            {
                //take in user input
                inp = ReadLine();
                string shape = "";
                //if user mistyped shape when adding print appropriate message
                if (inp.Equals("A") || inp.Equals("a")) { 
                    WriteLine("Forgot to type in shape");
                    WriteLine("List of valid shapes: \n ->Rectangle \n ->Circle \n ->Ellispe \n ->Line \n ->Polyline \n ->Polygon \n ->Path \n ->Text");
                    inp = "";
                }
                //split letter and shape if user wants to add shape
                else if (inp.Contains("A") || inp.Contains("a"))
                {
                    shape = inp.Substring(2);
                    inp = inp.Substring(0, 1).ToUpper();
                    shape = shape.Substring(0, 1).ToUpper() + shape.Substring(1).ToLower();
                } 
                else { inp = inp.ToUpper(); }
                //use switch statement to run through appropriate lines
                switch (inp)
                {
                    //A - adding shape
                    case "A":
                        //run through if statement if user has added their own boundaries for shape being inputted
                        if (shape.Contains("(") && shape.Contains(")"))
                        {
                            string points = getPoints(shape);
                            for(int i = 0; i < shape.Length; i++)
                            {
                                if(shape[i].Equals('(') || shape[i].Equals(' ')) { shape = shape.Substring(0, i); }
                            }
                            addShape(canvas, shape, points);
                        }
                        //if no boundaries inputted, just use default boundaries set already
                        else { addShape(shape, canvas); }
                        break;
                    //S - update style of shape
                    case "S":
                        WriteLine("Select Shape by index, to update style");
                        int index = 1;
                        //print out all shapes currently stored in canvas
                        foreach(Shape s in canvas)
                        {
                            WriteLine("Index " + index++ + ": " + s.name);
                        }
                        //let user select which shape they want to edit by selecting the index
                        int select = Convert.ToInt32(ReadLine());
                        //update shape in this method
                        updateShape(canvas, select);
                        break;
                    //V - show user what shapes they have already stored in an svg format
                    case "V":
                        WriteLine("Current svg file");
                        printCurrentSVG(canvas);
                        break;
                    //U - undo shape input
                    case "U":
                        //inform user that there are no more shapes to undo if canvas is empty
                        if(canvas.Count == 0) { WriteLine("There are no more shapes in the canvas"); }
                        else { undoShapeCanvas(canvas, undoRedo); }
                        break;
                    //T - add text into canvas
                    case "T":
                        addText(canvas);
                        break;
                    //R - redo shape into canvas
                    case "R":
                        //inform user theres no more shapes to add into canvas
                        if (undoRedo.getSize() == 0) { WriteLine("There are no more shapes to redo"); }
                        else { redoShapeCanvas(canvas, undoRedo); }
                        break;
                    //C - clear canvas
                    case "C":
                        //make sure reset caretaker so there isnt any mis-redo
                        canvas.Clear();
                        undoRedo = new Caretaker();
                        WriteLine("canvas cleared!");
                        break;
                    //Q - quit the loop and export canvas to an svg file
                    case "Q":
                        svg = createSVG(canvas);
                        //inform user that file has been succesfully saved
                        WriteLine("File canvas.svg has been succesfully saved!");
                        File.WriteAllText("canvas.svg", svg);
                        run = false;
                        break;
                    //tell user their input is invalid
                    default:
                        WriteLine("Invalid Input");
                        break;
                }
                inp = "";
            }
        }
        //method creates svg layout from current shapes inside canvas
        //return as string
        public static string createSVG(List<Shape> shape)
        {
            string start = "<svg width=\"1000\" height=\"1000\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">";
            string middle = "";
            string end = "</svg>";
            foreach (Shape s in shape)
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
        //use method to add shape in canvas
        //each shape has default inputs that is added to canvas
        public static void addShape(string s, List<Shape> canvas)
        {
            //use switch statement to add appropriate shape to canvas
            //use default ints for shapes
            //add shape into canvas
            Shape temp;
            switch (s)
            {
                case "Rectangle":
                    Rectangle rect = new Rectangle(150, 200, 100, 350);
                    temp = rect;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (X=" + rect.x + ",Y=" + rect.y + ",W=" + rect.w + ",H=" + rect.h + ") added to canvas.");
                    break;
                case "Circle":
                    Circle circ = new Circle(150, 275, 200);
                    temp = circ;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (X=" + circ.cx + ",Y=" + circ.cy + ",R=" + circ.rad + ") added to canvas.");
                    break;
                case "Ellipse":
                    Ellipse ell = new Ellipse(200, 150, 340, 150);
                    temp = ell;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (RX=" + ell.rx + ",RY=" + ell.rx + ",CX=" + ell.cy + ",CY= " + ell.cy + ") added to canvas.");
                    break;
                case "Line":
                    Line li = new Line(250, 450, 180, 69);
                    temp = li;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (X1=" + li.x1 + ",Y1=" + li.y1 + ",X2=" + li.x2+ ",Y2=" + li.x2 + ") added to canvas.");
                    break;
                case "Polyline":
                    Polyline line = new Polyline("0,100 50,25 50,75 100,0");
                    temp = line;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (Co-Ordinates=" + line.coord + ") added to canvas.");
                    break;
                case "Polygon":
                    Polygon gon = new Polygon("100,100 150,25 150,75 200,0");
                    temp = gon;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (Co-Ordinates=" + gon.coord + ") added to canvas.");
                    break;
                case "Path":
                    Path p = new Path("M 30 40 C 140 -30 180 90 20 160 L 120 160");
                    temp = p;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (Co-Ordinates=" + p.coord + ") added to canvas.");
                    break;
                default:
                    WriteLine("Invalid shape input");
                    WriteLine("List of valid shapes: \n ->Rectangle \n ->Circle \n ->Ellispe \n ->Line \n ->Polyline \n ->Polygon \n ->Path");
                    break;
            }
        }
        //finding int in string for points
        static int findPoint(string s)
        {
            for(int i = 0; i < s.Length; i++)
            {
                if(s[i].Equals('=')) { s = s.Substring(i + 1); }
            }
            return Int32.Parse(s);
        }
        //this add shape method includes the points created by the user
        static void addShape(List<Shape> canvas, string shape, string points)
        {
            Shape temp;
            int x;
            switch (shape)
            {
                case "Rectangle":
                    //only make array for shapes for rectangle, circle, ellipse, line
                    string[] p1 = points.Split(',');
                    Rectangle rect = new Rectangle(150, 200, 100, 350);
                    //for loop loops through array, to see which node correlating to each boundary of the shape
                    //i.e x value, y value... etc
                    for(int i = 0; i < p1.Length; i++)
                    {
                        if(p1[i].Contains('x'))
                        {
                            x = findPoint(p1[i]);
                            rect.x = x;
                        }
                        else if (p1[i].Contains('y'))
                        {
                            x = findPoint(p1[i]);
                            rect.y = x;
                        }
                        else if (p1[i].Contains('w'))
                        {
                            x = findPoint(p1[i]);
                            rect.w = x;
                        }
                        else
                        {
                            x = findPoint(p1[i]);
                            rect.h= x;
                        }
                    }
                    temp = rect;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (X=" + rect.x + ",Y=" + rect.y + ",W=" + rect.w + ",H=" + rect.h + ") added to canvas.");
                    break;
                case "Circle":
                    string[] p2 = points.Split(',');
                    Circle circ = new Circle(150, 275, 200);
                    for (int i = 0; i < p2.Length; i++)
                    {
                        if (p2[i].Contains('x'))
                        {
                            x = findPoint(p2[i]);
                            circ.cx = x;
                        }
                        else if (p2[i].Contains('y'))
                        {
                            x = findPoint(p2[i]);
                            circ.cy = x;
                        }
                        else
                        {
                            x = findPoint(p2[i]);
                            circ.rad = x;
                        }
                    }
                    temp = circ;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (X=" + circ.cx + ",Y=" + circ.cy + ",R=" + circ.rad + ") added to canvas.");
                    break;
                case "Ellipse":
                    string[] p3 = points.Split(',');
                    Ellipse ell = new Ellipse(200, 150, 340, 150);
                    for (int i = 0; i < p3.Length; i++)
                    {
                        if (p3[i].Contains("rx"))
                        {
                            x = findPoint(p3[i]);
                            ell.rx = x;
                        }
                        else if (p3[i].Contains("ry"))
                        {
                            x = findPoint(p3[i]);
                            ell.ry = x;
                        }
                        else if (p3[i].Contains("cx"))
                        {
                            x = findPoint(p3[i]);
                            ell.cx = x;
                        }
                        else
                        {
                            x = findPoint(p3[i]);
                            ell.cy = x;
                        }
                    }
                    temp = ell;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (RX=" + ell.rx + ",RY=" + ell.ry + ",CX=" + ell.cx + ",CY= " + ell.cy + ") added to canvas.");
                    break;
                case "Line":
                    string[] p4 = points.Split(',');
                    Line li = new Line(250, 450, 180, 69);
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (p4[i].Contains("x1"))
                        {
                            x = findPoint(p4[i]);
                            li.x1 = x;
                        }
                        else if (p4[i].Contains("y1"))
                        {
                            x = findPoint(p4[i]);
                            li.y1 = x;
                        }
                        else if (p4[i].Contains("x2"))
                        {
                            x = findPoint(p4[i]);
                            li.x2 = x;
                        }
                        else
                        {
                            x = findPoint(p4[i]);
                            li.y2 = x;
                        }
                    }
                    temp = li;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (X1=" + li.x1 + ",Y1=" + li.y1 + ",X2=" + li.x2 + ",Y2=" + li.x2 + ") added to canvas.");
                    break;
                case "Polyline":
                    for (int i = 0; i < points.Length; i++) { if (points[i].Equals('=')) { points = points.Substring(i + 1); } }
                    Polyline line = new Polyline(points);
                    temp = line;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (points=" + line.coord + ") added to canvas.");
                    break;
                case "Polygon":
                    for (int i = 0; i < points.Length; i++) { if (points[i].Equals('=')) { points = points.Substring(i + 1); } }
                    Polygon gon = new Polygon(points);
                    temp = gon;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (points=" + gon.coord + ") added to canvas.");
                    break;
                case "Path":
                    for(int i = 0; i < points.Length; i++) { if (points[i].Equals('=')) { points = points.Substring(i + 1); } }
                    Path p = new Path(points);
                    temp = p;
                    canvas.Add(temp);
                    WriteLine(temp.name + " (d=" + p.coord + ") added to canvas.");
                    break;
                default:
                    WriteLine("Invalid shape input");
                    WriteLine("List of valid shapes: \n ->Rectangle(x,y,w,h) \n ->Circle(x,y,r) \n ->Ellispe(rx,ry,cx,cy) \n ->Line(x1,y1,x2,y2) \n ->Polyline(points) \n ->Polygon(points) \n ->Path(points)");
                    break;
            }
        }
        //undo method
        static void undoShapeCanvas(List<Shape> canvas, Caretaker undoRedo)
        {
            //create memento state
            Memento state = new Memento();
            //create shape variable and store last shape in canvas
            Shape shape = canvas[canvas.Count - 1];
            //set the state to the last shape
            state.setShape(shape);
            //remove shape in cavas
            canvas.Remove(shape);
            //then add that state to the caretaker
            undoRedo.addState(state);
            WriteLine(shape.name + " has been removed from canvas");
        }
        //redo method
        static void redoShapeCanvas(List<Shape> canvas, Caretaker undoRedo)
        {
            //create memento variable and get last state in caretaker
            Memento state = undoRedo.getState();
            //create shape variable and store the state
            Shape shape = state.getShape();
            //add shape in the canvas
            canvas.Add(shape);
            WriteLine(shape.name + " has been added back to the canvas");
        }
        //update shape's style method
        static void updateShape(List<Shape> canvas, int select)
        {
            //get shape in canvas
            Shape s = canvas[select - 1];
            //update fill, stroke-width & stroke values
            WriteLine("Update values for " + s.name);
            WriteLine("Current stroke width length: " + s.strokeWidth + "\nInput new value: ");
            s.strokeWidth = Convert.ToInt32(ReadLine());
            WriteLine("Current stroke value: " + s.stroke + "\nInput new value: ");
            s.stroke = ReadLine();
            WriteLine("Current fill value: " + s.fill + "\nInput new value: ");
            s.fill = ReadLine();
            canvas[select - 1] = s;
            WriteLine("New style for " + s.name + ": (Stroke Width=" + s.strokeWidth + ", Stroke=" + s.stroke + ", Fill=" + s.fill + ")");
        }
        //print svg method
        //this shows user their current canvas they have made
        static void printCurrentSVG(List<Shape> canvas)
        {
            String SVG = createSVG(canvas);
            WriteLine(SVG);
        }
        //get points method is used to find the user inputs for the boundaries of each of the shapes made
        static string getPoints(string s)
        {
            //essentially using a while loop to make a substring above the '('
            //then using a simple command to remove the other ')'
            string temp = "";
            for(int i = 0; i < s.Length; i++) { if(s[i].Equals('(')) { temp = s.Substring(i + 1); } }
            temp = temp.Substring(0, temp.Length - 1);
            return temp;
        }
        //add text method
        static void addText(List<Shape> canvas)
        {
            //allow user to type in string for the text
            WriteLine("Enter a string for text object: ");
            string s = ReadLine();
            //class name is used to add the different styles for the text
            WriteLine("Enter class name for text object: ");
            string n = ReadLine();
            Text xt = new Text(s, n);
            //give the user the option to update the style of the text
            WriteLine("Would you like to edit your text?\n(Answer with Y/N)");
            string ans = ReadLine();
            //user is able to upadte the font, x & y coordinate, fill, stroke-width & stroke
            if (ans.Equals("Y") || ans.Equals("y"))
            {
                Shape tempTxt = xt;
                WriteLine("Current font: " + xt.font + "\nInput new font: ");
                xt.font = ReadLine();
                WriteLine("Current x co-ordinate: " + xt.x + "\nInput new x co-ordinate");
                xt.x = Convert.ToInt32(ReadLine());
                WriteLine("Current y co-ordinate: " + xt.y + "\nInput new y co-ordinate");
                xt.y = Convert.ToInt32(ReadLine());
                WriteLine("Current stroke width length: " + tempTxt.strokeWidth + "\nInput new value: ");
                tempTxt.strokeWidth = Convert.ToInt32(ReadLine());
                WriteLine("Current stroke value: " + tempTxt.stroke + "\nInput new value: ");
                tempTxt.stroke = ReadLine();
                WriteLine("Current fill value: " + tempTxt.fill + "\nInput new value: ");
                tempTxt.fill = ReadLine();
            }
            //add text inside canvas
            canvas.Add(xt);
            WriteLine("Text: " + xt.text + ", is added to canvas");
        }
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
        public Circle (int x, int y, int r)
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
    class Text : Shape
    {
        public string font { get; set; }
        public string text { get; set; }
        public string className { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Text(string s, string c)
        {
            this.name = "Text";
            this.text = s;
            this.className = c;
            this.x = 65;
            this.y = 55;
            this.font = "italic 15px serif";
            this.fill = "cadetblue";
            this.stroke = "grey";
            this.strokeWidth = 1;
        }
        public override string getCode()
        {
            string s1 = "";
            s1 = "    <style>";
            s1 += "\n      ." + this.className + " { font: " + this.font + "; fill: " + this.fill + "; stroke: " + this.stroke + "; stroke-width: " + this.strokeWidth + "; }";
            s1 += "\n    </style>";
            s1 += "\n    <text x=\"" + this.x + "\" y=\"" + this.y + "\" class=\"" + this.className + "\">" + this.text + "</text>";
            return s1;
        }
    }
    //Memento Design Pattern
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
    //caretaker is used to store memento inside in
    class Caretaker
    {
        List<Memento> undoRedo = new List<Memento>();
        Memento state = new Memento();
        public Memento getState()
        {
            this.state = this.undoRedo[undoRedo.Count-1];
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
