using System;
using System.Drawing;
using System.Windows.Forms;

namespace NeuralNetworkExample2
{
    public class Graphic
    {
        Graphics g;
        Pen pen = new Pen(Color.Red, 1);
        int GraphicStep;

        int x_current = 0;
        float x_previous = 0;
        float y_previous = 0;

        Panel pnl_graphic;

        public Graphic(Panel panelToDrawGraphicOn, int graphicStep)
        {
            pnl_graphic = panelToDrawGraphicOn;
            g = pnl_graphic.CreateGraphics();

            GraphicStep = graphicStep;
        }

        /// <summary>
        /// Draw new point on graphic
        /// </summary>
        /// <param name="value"> percent </param>
        public void DrawPoint(float value)
        {
            value = pnl_graphic.Height * value;
            x_current += GraphicStep;

            if (x_current > pnl_graphic.Width - GraphicStep)
            {
                ClearGraphic();
            }

            value = (int)value;

            DrawPoint(x_current, value - 10);
        }

        void ClearGraphic()
        {
            g.Clear(pnl_graphic.BackColor);

            x_current = GraphicStep;
            x_previous = 0;
        }

        void DrawPoint(float x, float y)
        {
            g.DrawLine(pen, x_previous, y_previous, x, Math.Abs(y));

            x_previous = x;
            y_previous = y;
        }
    }
}
