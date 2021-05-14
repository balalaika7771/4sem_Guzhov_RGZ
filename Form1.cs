using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using System.IO;


namespace openRGZ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }
        TCube3D COW = new TCube3D("C:\\Project\\cow.obj");//для создания кубика нам нужно дать ему путь к файлу из которого он будет читать координаты своих вершин
        double teta = 4, phi =-20, zer = 0;   //углы для поворота
		double RRRX = -5, RRRY = -6, RRRZ = -19;//перемещение кубика по экрану 
		double zoom = 0.01;//увеличение кубика
		private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.KeyValue == (char)Keys.K)//тут просто реакция на нажатие клавиш
			{
				teta += 1;
			}
			if (e.KeyValue == (char)Keys.J)
			{
				teta -= 1;
			}

			if (e.KeyValue == (char)Keys.I)
			{
				phi -= 1;
			}
			if (e.KeyValue == (char)Keys.U)
			{
				phi += 1;
			}

			if (e.KeyValue == (char)Keys.M)
			{
				zer += 1;
			}
			if (e.KeyValue == (char)Keys.N)
			{
				zer -= 1;
			}


			if (e.KeyValue == (char)Keys.W)
			{
				RRRX += 1;
			}
			if (e.KeyValue == (char)Keys.S)
			{
				RRRX -= 1;
			}

			if (e.KeyValue == (char)Keys.D)
			{
				RRRY += 1;
			}
			if (e.KeyValue == (char)Keys.A)
			{
				RRRY -= 1;
			}

			if (e.KeyValue == (char)Keys.Q)
			{
				RRRZ += 1;
			}
			if (e.KeyValue == (char)Keys.E)
			{
				RRRZ -= 1;
			}


			if (e.KeyValue == (char)Keys.Z)
			{
				zoom += 0.0001;
			}
			if (e.KeyValue == (char)Keys.X)
			{
				zoom -= 0.0001;
			}
		}

        
		



        private void openGLControl1_OpenGLDraw(object sender, EventArgs e)
        {
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
			// Создаем экземпляр
			OpenGL gl = this.openGLControl1.OpenGL;
			// Очистка экрана и буфера глубин
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
			gl.LoadIdentity();

			gl.Translate(RRRX, RRRY, RRRZ);
			gl.Rotate(teta, 1.0f, 0.0f, 0.0f);
			gl.Rotate(phi, 0.0f, 1.0f, 0.0f);
			gl.Rotate(zer, 0.0f, 0.0f, 1.0f);

			gl.Begin(OpenGL.GL_TRIANGLES);
			if (COW.color.Count > 0)
			{
				for (int i = 0; i < COW.polygons.Count; i++)
				{
					gl.Color(COW.color[COW.polygons[i].Item1 - 1].X, COW.color[COW.polygons[i].Item1 - 1].Y, COW.color[COW.polygons[i].Item1 - 1].Z);
					gl.Vertex(COW.vertices[COW.polygons[i].Item1 - 1].X * zoom, COW.vertices[COW.polygons[i].Item1 - 1].Y * zoom, COW.vertices[COW.polygons[i].Item1 - 1].Z * zoom);
					gl.Color(COW.color[COW.polygons[i].Item2 - 1].X, COW.color[COW.polygons[i].Item2 - 1].Y, COW.color[COW.polygons[i].Item2 - 1].Z);
					gl.Vertex(COW.vertices[COW.polygons[i].Item2 - 1].X * zoom, COW.vertices[COW.polygons[i].Item2 - 1].Y * zoom, COW.vertices[COW.polygons[i].Item2 - 1].Z * zoom);
					gl.Color(COW.color[COW.polygons[i].Item3 - 1].X, COW.color[COW.polygons[i].Item3 - 1].Y, COW.color[COW.polygons[i].Item3 - 1].Z);
					gl.Vertex(COW.vertices[COW.polygons[i].Item3 - 1].X * zoom, COW.vertices[COW.polygons[i].Item3 - 1].Y * zoom, COW.vertices[COW.polygons[i].Item3 - 1].Z * zoom);
				}
            }
            else
            {
				for (int i = 0; i < COW.polygons.Count; i++)
				{
					gl.Color(1, 0, 0);
					gl.Vertex(COW.vertices[COW.polygons[i].Item1 - 1].X * zoom, COW.vertices[COW.polygons[i].Item1 - 1].Y * zoom, COW.vertices[COW.polygons[i].Item1 - 1].Z * zoom);
					gl.Color(0, 0, 1);
					gl.Vertex(COW.vertices[COW.polygons[i].Item2 - 1].X * zoom, COW.vertices[COW.polygons[i].Item2 - 1].Y * zoom, COW.vertices[COW.polygons[i].Item2 - 1].Z * zoom);
					gl.Color(0, 1, 0);
					gl.Vertex(COW.vertices[COW.polygons[i].Item3 - 1].X * zoom, COW.vertices[COW.polygons[i].Item3 - 1].Y * zoom, COW.vertices[COW.polygons[i].Item3 - 1].Z * zoom);
				}
			}

		

			gl.End();
			// Контроль полной отрисовки следующего изображения
			gl.Flush();

		}
	}

	class Vertex
	{
		public double X { set; get; }
		public double Y { set; get; }
		public double Z { set; get; }

		public Vertex(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

	};

	class TCube3D
	{
		public List<Vertex> vertices = new List<Vertex>();  // список вершин
		public List<Vertex> color = new List<Vertex>();  // закраска
		public List<Tuple<int, int, int>> polygons = new List<Tuple<int, int, int>>();  // список номеров полигонов

		public TCube3D(string file_name)
		{
			string[] lines = File.ReadAllLines(file_name);
			foreach (string line in lines)
			{
				// строки с вершинами
				if (line.ToLower().StartsWith("v "))
				{
					var vx = line.Split(' ')
						.Skip(1)
						.Select(v => Double.Parse(v.Replace('.', ',')))
						.ToArray();
					vertices.Add(new Vertex(vx[0], vx[1], vx[2]));
				}
				// строки с номерами
				else if (line.ToLower().StartsWith("f"))
				{
					string str;
					bool key = true;
					if(line[line.Length-1] == ' ')
                    {
						str = line.Remove(line.Length - 1, 1);
                    }
                    else
                    {
						str = line;
                    }

					while (key)
					{
						int h = 0, k  = 0;
						for (int i = 0; i < str.Length; i++)
						{
							if(str[i] == '/')
                            {
								h = i;
								int j = i;
								while(str[j] != ' ' && j < str.Length-1)
                                {
									k++;
									j++;
                                }
								if(j == str.Length - 1)
                                {
									k++;
                                }
								str = str.Remove(h, k);
								break;
							}
							if(i == str.Length - 1)
                            {
								key = false;
							}
						}
						

					}
					var vx = str.Split(' ')
						.Skip(1)
						.Select(v => int.Parse(v))
						.ToArray();
					polygons.Add(new Tuple<int, int, int>((vx[0]), (vx[1]), (vx[2])));
				}
				if (line.StartsWith("vt"))
				{
					var vx = line.Split(' ')
						.Skip(1)
						.Select(v => Double.Parse(v.Replace('.', ',')))
						.ToArray();
					color.Add(new Vertex(vx[0], vx[1], vx[2]));
				}
			}
		}

		
	};
}

