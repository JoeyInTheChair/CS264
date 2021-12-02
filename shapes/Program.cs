using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;

namespace CS264Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            //list of shapes
            LinkedList<Shape> shapes = new LinkedList<Shape>();
            string ans = "";
            int pos = 1; //zIndex
            string svg = "";
            //bool value is used for while loops
            bool cont = true;
            WriteLine("All Possible Shapes: \n->Rectangle\n->Circle\n->Ellipse\n->Line\n->Polyline\n->Polygon\n->Path");
            while (cont)
            {
                Shape tempShape = new Shape();
                //ask user to type out shape
                WriteLine("Enter Shape: ");
                string inp = ReadLine();
                if (String.IsNullOrEmpty(inp)) { cont = false; } //stop loop when user is finished
                else
                {
                    inp = inp.ToLower();
                    inp = inp.Substring(0, 1).ToUpper() + inp.Substring(1);
                    //user fills out appropriate information for various shapes
                    if (inp.Equals("Rectangle"))
                    {
                        WriteLine("Set Boundaries for Rectangle: ");
                        WriteLine("Position X: ");
                        int x = Convert.ToInt32(ReadLine());
                        WriteLine("Position Y: ");
                        int y = Convert.ToInt32(ReadLine());
                        WriteLine("Width of Rectangle: ");
                        int w = Convert.ToInt32(ReadLine());
                        WriteLine("Height of Rectangle: ");
                        int h = Convert.ToInt32(ReadLine());
                        tempShape.rectangle(inp, x, y, w, h, pos++);
                        shapes.AddFirst(tempShape);
                    }
                    else if (inp.Equals("Circle"))
                    {
                        WriteLine("Set Boundaries for Circle: ");
                        WriteLine("Radius: ");
                        int r = Convert.ToInt32(ReadLine());
                        WriteLine("Position CX: ");
                        int x = Convert.ToInt32(ReadLine());
                        WriteLine("Position CY: ");
                        int y = Convert.ToInt32(ReadLine());
                        tempShape.circle(inp, r, x, y, pos++);
                        shapes.AddFirst(tempShape);
                    }
                    else if (inp.Equals("Ellipse"))
                    {
                        WriteLine("Set boundaries for Ellipse");
                        WriteLine("X radius of Ellipse: ");
                        int rx = Convert.ToInt32(ReadLine());
                        WriteLine("Y radius of Ellipse: ");
                        int ry = Convert.ToInt32(ReadLine());
                        WriteLine("Center X of Ellipse: ");
                        int cx = Convert.ToInt32(ReadLine());
                        WriteLine("Center Y of Ellipse: ");
                        int cy = Convert.ToInt32(ReadLine());
                        tempShape.ellipse(inp, rx, ry, cx, cy, pos++);
                        shapes.AddFirst(tempShape);
                    }
                    else if (inp.Equals("Line"))
                    {
                        WriteLine("Set boundaries for Line");
                        WriteLine("X Co-ordinate for Point 1: ");
                        int x1 = Convert.ToInt32(ReadLine());
                        WriteLine("Y Co-ordinate for Point 1: ");
                        int y1 = Convert.ToInt32(ReadLine());
                        WriteLine("X Co-ordinate for Point 2: ");
                        int x2 = Convert.ToInt32(ReadLine());
                        WriteLine("Y Co-ordinate for Point 2: ");
                        int y2 = Convert.ToInt32(ReadLine());
                        tempShape.line(inp, x1, y1, x2, y2, pos++);
                        shapes.AddFirst(tempShape);
                    }
                    else if (inp.Equals("Polyline"))
                    {
                        WriteLine("Set boundaries for Polyline");
                        WriteLine("Set the points for Polyline \n(i.e 10, 20 25, 35 90, ... , 23 420, 69) ");
                        string coord = ReadLine();
                        tempShape.poly(inp, coord, pos++);
                        shapes.AddFirst(tempShape);
                    }
                    else if (inp.Equals("Polygon"))
                    {
                        WriteLine("Set boundaries for Polygon");
                        WriteLine("Set the points for Polygon \n(i.e 10, 20 25, 35 90, ... , 23 420, 69) ");
                        string coord = ReadLine();
                        tempShape.poly(inp, coord, pos++);
                        shapes.AddFirst(tempShape);
                    }
                    else if (inp.Equals("Path"))
                    {
                        WriteLine("Set boundaries for Path");
                        WriteLine("Set commands for Path \n'M' move to\n'L' line to\n'V' vertical line to\n'H' horizontal line to\n'C' curve to\n'S' smooth curve to\n'T' quadratic Bezier curve\n'A' elliptical arc\n'Z' close path\n(i.e M 30 40 C 140 -30 180 90 20 160 L 120 160): ");
                        string coord = ReadLine();
                        tempShape.poly(inp, coord, pos++);
                        shapes.AddFirst(tempShape);
                    }
                    else { WriteLine("Invalid Input \nTry Again"); }
                    WriteLine("Press Enter When Finished");
                }
                inp = "";
            }
            Console.Clear();
            cont = true;
            WriteLine("-------------------------------------------------------------------------------------------------------\n");
            WriteLine("Your List of Shapes: ");
            foreach (Shape s in shapes) { s.getShape(); }
            WriteLine("-------------------------------------------------------------------------------------------------------\n");
            WriteLine("Would you like to swap any of the shapes' positions? \n(Answer with: Y/N)");
            ans = ReadLine().ToUpper();
            if (ans.Equals("Y"))
            {
                while (cont)
                {
                    WriteLine("Which shape would you like to swap positions with?\n(Select by typing in index)");
                    WriteLine("Select 1st Index:");
                    int indOne = Convert.ToInt32(ReadLine());
                    WriteLine("Select 2nd Index:");
                    int indTwo = Convert.ToInt32(ReadLine());
                    swapPositions(shapes, indOne, indTwo);
                    WriteLine("New Positioned List:");
                    foreach (Shape s in shapes) { s.getShape(); }
                    WriteLine("Would you like to continue to update or delete shapes? \nPress Enter if finished else Press 1 then Enter to continue");
                    if (String.IsNullOrEmpty(ReadLine())) { cont = false; }
                }
            }
            Console.Clear();
            WriteLine("-------------------------------------------------------------------------------------------------------\n");
            cont = true;
            //ask user if they would like to update or delete shapes in list
            WriteLine("Would you like to update or delete any shapes? \n(Answer with: Y/N)");
            ans = "";
            ans = ReadLine().ToUpper();
            if (ans.Equals("Y"))
            {
                //create loop to continously ask user to update or delete shapes
                //stop loop if user is finished
                while (cont)
                {
                    WriteLine("Select Index: ");
                    foreach (Shape s in shapes) { s.getShape(); }
                    int index = Convert.ToInt32(ReadLine());
                    WriteLine("Select Update or Delete: \n(Answer with: U/D)");
                    string res = ReadLine();
                    if (res.Equals("D") || res.Equals("d")) { deleteShape(shapes, index); }
                    else { updateList(shapes, index); }
                    svg = createSVG(shapes);
                    WriteLine("Would you like to continue to update or delete shapes? \nPress Enter if finished else Press 1 then Enter to continue");
                    if (String.IsNullOrEmpty(ReadLine())) { cont = false; }
                }
            }
            else
            {
                svg = createSVG(shapes);
            }
            Console.Clear();
            WriteLine("-------------------------------------------------------------------------------------------------------\n");
            //print out SVG
            File.WriteAllText("shapes.svg", svg);
            WriteLine("Your SVG File: " + "\n" + svg + "\nYour SVG file is saved as shapes.svg");
        }
        //create SVG file method
        public static string createSVG(LinkedList<Shape> shape)
        {
            string start = "<svg width=\"300\" height=\"300\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">";
            string middle = "";
            string end = "</svg>";
            foreach (Shape s in shape)
            {
                if (s.getName().Equals("Rectangle")) { middle += "\n" + s.getRectangle(); }
                else if (s.getName().Equals("Circle")) { middle += "\n" + s.getCircle(); }
                else if (s.getName().Equals("Ellipse")) { middle += "\n" + s.getEllipse(); }
                else if (s.getName().Equals("Line")) { middle += "\n" + s.getLine(); }
                else if (s.getName().Equals("Polyline")) { middle += "\n" + s.getPolyline(); }
                else if (s.getName().Equals("Polygon")) { middle += "\n" + s.getPolygon(); }
                else if (s.getName().Equals("Path")) { middle += "\n" + s.getPath(); }
            }
            return start + middle + "\n" + end;
        }
        public static void swapPositions(LinkedList<Shape> shape, int x, int y)
        {
            int pos = 0;
            Shape[] temp = new Shape[shape.Count];
            foreach (Shape s in shape) { temp[pos++] = s; }
            Shape tempShape = temp[temp.Length - x];
            temp[temp.Length - x] = temp[temp.Length - y];
            temp[temp.Length - y] = tempShape;
            temp[temp.Length - x].setIndex(x);
            temp[temp.Length - y].setIndex(y);
            shape.Clear();
            for (int i = 0; i < temp.Length; i++) shape.AddLast(temp[i]);
        }
        public static void deleteShape(LinkedList<Shape> shape, int index)
        {
            Shape temp = new Shape();
            foreach (Shape s in shape)
            {
                if (index == s.getIndex()) { temp = s; }
            }
            shape.Remove(temp);
        }
        public static void updateList(LinkedList<Shape> shape, int index)
        {
            foreach (Shape s in shape)
            {
                if (index == s.getIndex())
                {
                    WriteLine("Current Value:");
                    if (s.getName().Equals("Rectangle"))
                    {
                        WriteLine("x value: " + s.getX1() + "\ny value: " + s.getY1() + "\nwidth value: " + s.getX2() + "\nheight value: " + s.getY2() + "\nstroke width value: " + s.getStokeWidth() + "\nfill value: " + s.getFill() + "\nstroke value: " + s.getStroke());
                        WriteLine("Enter New Values: ");
                        WriteLine("New x value: ");
                        s.setX1(Convert.ToInt32(ReadLine()));
                        WriteLine("New y value: ");
                        s.setY1(Convert.ToInt32(ReadLine()));
                        WriteLine("New width value: ");
                        s.setX2(Convert.ToInt32(ReadLine()));
                        WriteLine("New height value: ");
                        s.setY2(Convert.ToInt32(ReadLine()));
                        WriteLine("New stroke width value: ");
                        s.setStrokeWidth(Convert.ToInt32(ReadLine()));
                        WriteLine("New fill value (colour): ");
                        s.setFill(ReadLine());
                        WriteLine("New stroke value (colour): ");
                        s.setStroke(ReadLine());
                    }
                    else if (s.getName().Equals("Circle"))
                    {
                        WriteLine("cx value: " + s.getX1() + "\ncy value: " + s.getY1() + "\nradius value: " + s.getRad() + "\nstroke width value: " + s.getStokeWidth() + "\nfill value: " + s.getFill() + "\nstroke value: " + s.getStroke());
                        WriteLine("Enter New Values: ");
                        WriteLine("New cx value: ");
                        s.setX1(Convert.ToInt32(ReadLine()));
                        WriteLine("New cy value: ");
                        s.setY1(Convert.ToInt32(ReadLine()));
                        WriteLine("New radius value: ");
                        s.setRad(Convert.ToInt32(ReadLine()));
                        WriteLine("New stroke width value: ");
                        s.setStrokeWidth(Convert.ToInt32(ReadLine()));
                        WriteLine("New fill value (colour): ");
                        s.setFill(ReadLine());
                        WriteLine("New stroke value (colour): ");
                        s.setStroke(ReadLine());
                    }
                    else if (s.getName().Equals("Ellipse"))
                    {
                        WriteLine("cx value: " + s.getX1() + "\ncy value: " + s.getY1() + "\nrx value: " + s.getX2() + "\nry value: " + s.getY2() + "\nstroke width value: " + s.getStokeWidth() + "\nfill value: " + s.getFill() + "\nstroke value: " + s.getStroke());
                        WriteLine("Enter New Values: ");
                        WriteLine("New cx value: ");
                        s.setX1(Convert.ToInt32(ReadLine()));
                        WriteLine("New cy value: ");
                        s.setY1(Convert.ToInt32(ReadLine()));
                        WriteLine("New rx value: ");
                        s.setX2(Convert.ToInt32(ReadLine()));
                        WriteLine("New ry value: ");
                        s.setY2(Convert.ToInt32(ReadLine()));
                        WriteLine("New stroke width value: ");
                        s.setStrokeWidth(Convert.ToInt32(ReadLine()));
                        WriteLine("New fill value (colour): ");
                        s.setFill(ReadLine());
                        WriteLine("New stroke value (colour): ");
                        s.setStroke(ReadLine());
                    }
                    else if (s.getName().Equals("Line"))
                    {
                        WriteLine("x1 value: " + s.getX1() + "\ny1 value: " + s.getY1() + "\nx2 value: " + s.getX2() + "\ny2 value: " + s.getY2() + "\nstroke width value: " + s.getStokeWidth() + "\nfill value: " + s.getFill() + "\nstroke value: " + s.getStroke());
                        WriteLine("Enter New Values: ");
                        WriteLine("New x1 value: ");
                        s.setX1(Convert.ToInt32(ReadLine()));
                        WriteLine("New y1 value: ");
                        s.setY1(Convert.ToInt32(ReadLine()));
                        WriteLine("New x2 value: ");
                        s.setX2(Convert.ToInt32(ReadLine()));
                        WriteLine("New y2 value: ");
                        s.setY2(Convert.ToInt32(ReadLine()));
                        WriteLine("New stroke width value: ");
                        s.setStrokeWidth(Convert.ToInt32(ReadLine()));
                        WriteLine("New fill value (colour): ");
                        s.setFill(ReadLine());
                        WriteLine("New stroke value (colour): ");
                        s.setStroke(ReadLine());
                    }
                    else if (s.getName().Equals("Polyline"))
                    {
                        WriteLine("points: " + s.getCoord() + "\nstroke width value: " + s.getStokeWidth() + "\nfill value: " + s.getFill() + "\nstroke value: " + s.getStroke());
                        WriteLine("Enter New Values: ");
                        WriteLine("New points: ");
                        s.setCoord(ReadLine());
                        WriteLine("New stroke width value: ");
                        s.setStrokeWidth(Convert.ToInt32(ReadLine()));
                        WriteLine("New fill value (colour): ");
                        s.setFill(ReadLine());
                        WriteLine("New stroke value (colour): ");
                        s.setStroke(ReadLine());
                    }
                    else if (s.getName().Equals("Polygon"))
                    {
                        WriteLine("points: " + s.getCoord() + "\nstroke width value: " + s.getStokeWidth() + "\nfill value: " + s.getFill() + "\nstroke value: " + s.getStroke());
                        WriteLine("Enter New Values: ");
                        WriteLine("New points: ");
                        s.setCoord(ReadLine());
                        WriteLine("New stroke width value: ");
                        s.setStrokeWidth(Convert.ToInt32(ReadLine()));
                        WriteLine("New fill value (colour): ");
                        s.setFill(ReadLine());
                        WriteLine("New stroke value (colour): ");
                        s.setStroke(ReadLine());
                    }
                    else if (s.getName().Equals("Path"))
                    {
                        WriteLine("d value: " + s.getCoord() + "\nstroke width value: " + s.getStokeWidth() + "\nfill value: " + s.getFill() + "\nstroke value: " + s.getStroke());
                        WriteLine("Enter New Values: ");
                        WriteLine("New d value: ");
                        s.setCoord(ReadLine());
                        WriteLine("New stroke width value: ");
                        s.setStrokeWidth(Convert.ToInt32(ReadLine()));
                        WriteLine("New fill value (colour): ");
                        s.setFill(ReadLine());
                        WriteLine("New stroke value (colour): ");
                        s.setStroke(ReadLine());
                    }
                }
            }
        }
    }

    //creating shape classes
    class Shape
    {
        //shape class variables
        private string name;
        private int index;
        private int x1; //cx(circle), rx(ellipse), x1(line)
        private int y1; //cy(circle), ry(ellipse), y1(line) 
        private int x2; //cx(ellipse), x2(line)
        private int y2; //cy(ellipse), y2(line)
        private int rad;
        private string coord;
        private int strokeWidth;
        private string stroke;
        private string fill;
        //each shape method
        public void rectangle(string name, int x, int y, int w, int h, int i)
        {
            this.name = name;
            this.index = i;
            this.x1 = x;
            this.y1 = y;
            this.x2 = w;
            this.y2 = h;
            this.strokeWidth = 1;
            this.stroke = "black";
            this.fill = "grey";
        }
        public void circle(string name, int r, int x, int y, int i)
        {
            this.name = name;
            this.index = i;
            this.x1 = x;
            this.y1 = y;
            this.rad = r;
            this.strokeWidth = 1;
            this.stroke = "black";
            this.fill = "grey";
        }
        public void ellipse(string name, int radX, int radY, int x, int y, int i)
        {
            this.name = name;
            this.index = i;
            this.x1 = radX;
            this.y1 = radY;
            this.x2 = x;
            this.y2 = y;
            this.strokeWidth = 1;
            this.stroke = "black";
            this.fill = "grey";
        }
        public void line(string name, int a, int b, int c, int d, int i)
        {
            this.name = name;
            this.index = i;
            this.x1 = a;
            this.y1 = b;
            this.x2 = c;
            this.y2 = d;
            this.strokeWidth = 1;
            this.stroke = "black";
            this.fill = "grey";
        }
        public void poly(string name, string s, int i)
        {
            this.name = name;
            this.index = i;
            this.coord = s;
            this.strokeWidth = 1;
            this.stroke = "black";
            this.fill = "grey";
        }
        //print out z index and shape
        public void getShape()
        {
            WriteLine("Index: " + this.index + ", Name: " + this.name);
        }

        //getters and setters
        public void setName(String n) { this.name = n; }
        public void setIndex(int i) { this.index = i; }
        public void setX1(int x) { this.x1 = x; }
        public void setY1(int y) { this.y1 = y; }
        public void setX2(int x) { this.x2 = x; }
        public void setY2(int y) { this.y2 = y; }
        public void setRad(int r) { this.rad = r; }
        public void setCoord(string s) { this.coord = s; }
        public void setStrokeWidth(int w) { this.strokeWidth = w; }
        public void setFill(string f) { this.fill = f; }
        public void setStroke(string s) { this.stroke = s; }
        public string getName() { return this.name; }
        public int getIndex() { return this.index; }
        public int getX1() { return this.x1; }
        public int getY1() { return this.y1; }
        public int getX2() { return this.x2; }
        public int getY2() { return this.y2; }
        public int getRad() { return this.rad; }
        public string getCoord() { return this.coord; }
        public int getStokeWidth() { return this.strokeWidth; }
        public string getFill() { return this.fill; }
        public string getStroke() { return this.stroke; }

        //method allows to fill in appropriate line for SVG file
        public string getRectangle()
        {
            return "    <rect x=\"" + this.x1 + "\" y=\"" + this.y1 + "\" width=\"" + this.x2 + "\" height=\"" + this.y2 + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
        }
        public string getCircle()
        {
            return "    <circle cx=\"" + this.x1 + "\" cy=\"" + this.y1 + "\" r=\"" + this.rad + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
        }
        public string getEllipse()
        {
            return "    <ellipse cx=\"" + this.x1 + "\" cy=\"" + this.y1 + "\" rx=\"" + this.x2 + "\" ry=\"" + this.y2 + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
        }
        public string getLine()
        {
            return "    <line x1=\"" + this.x1 + "\" y1=\"" + this.y1 + "\" x2=\"" + this.x2 + "\" y2=\"" + this.y2 + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
        }
        public string getPolyline()
        {
            return "    <polyline fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" points=\"" + this.coord + "\"/>";
        }
        public string getPolygon()
        {
            return "    <polygon fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\" points=\"" + this.coord + "\"/>";
        }
        public string getPath()
        {
            return "    <path d=\"" + this.coord + "\" fill=\"" + this.fill + "\" stroke=\"" + this.stroke + "\" stroke-width=\"" + this.strokeWidth + "\"/>";
        }
    }
}