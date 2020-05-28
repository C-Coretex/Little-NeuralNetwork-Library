using System.Drawing;
using System.Windows.Forms;

namespace NeuralNetworkExample2
{
    public class Drawer
    {
        public Bitmap bmp = new Bitmap(100, 100);

        static Color defaultColor = Color.Black; //Default color
        Point CurrentPoint; //Current Position
        Point PrevPoint; //Previous Position
        bool isMousePressed;
        Graphics g;
        Graphics g1;
        Pen p = new Pen(defaultColor, 3);

        Panel panelToDrawOn;

        public Drawer(Panel panel)
        {
            this.panelToDrawOn = panel;

            g = panelToDrawOn.CreateGraphics();
            g1 = Graphics.FromImage(bmp);

            panelToDrawOn.MouseMove += panel_MouseMove;
            panelToDrawOn.MouseDown += panel_MouseDown;
            panelToDrawOn.MouseUp += panel_MouseUp;
            panelToDrawOn.Paint += panel_Paint;
        }

        public void Clear(Color backcolor)
        {
            g.Clear(panelToDrawOn.BackColor);
            g1.Clear(panelToDrawOn.BackColor);
        }

        #region draw events
        void panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Save();
        }

        void panel_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePressed = false;
        }

        void panel_MouseDown(object sender, MouseEventArgs e)
        {
            isMousePressed = true;
            CurrentPoint = e.Location;
        }

        void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMousePressed)
            {
                return;
            }

            PrevPoint = CurrentPoint;
            CurrentPoint = e.Location;
            g.DrawEllipse(p, PrevPoint.X, PrevPoint.Y, 3, 3);
            g1.DrawEllipse(p, PrevPoint.X, PrevPoint.Y, 3, 3);
        }
        #endregion
    }
}
