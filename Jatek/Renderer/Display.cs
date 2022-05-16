using Jatek.Controller;
using Jatek.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Jatek.Renderer
{
    public class Display : FrameworkElement
    {
        IGameModel model;
        ImageBrush iceBrush;
        ImageBrush ice1Brush;
        ImageBrush ice2Brush;
        ImageBrush ice3Brush;
        ImageBrush ice4Brush;
        ImageBrush ice5Brush;
        ImageBrush penguinBrush;
        ImageBrush garbageBrush;
        ImageBrush bulletfishBrush;
        ImageBrush hpfishBrush;
        ImageBrush sealBrush;
        ImageBrush bulletBrush;
        ImageBrush bgBrush;
        Size size;

        public int bullets() { return model.BulletNumber; }
        public int lives;



        public void Resize(Size size)
        {
            this.size = size;
        }

        public void SetupModel(IGameModel model)
        {
            iceBrush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "ice.bmp"),
                UriKind.RelativeOrAbsolute)));
            ice1Brush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "ice1.bmp"),
                UriKind.RelativeOrAbsolute)));
            ice2Brush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "ice2.png"),
                UriKind.RelativeOrAbsolute)));
            ice3Brush = new ImageBrush(new BitmapImage(new Uri
                (Path.Combine("Images", "ice3.png"),
                UriKind.RelativeOrAbsolute)));
            ice4Brush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "ice4.png"),
                UriKind.RelativeOrAbsolute)));
            ice5Brush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "ice5.png"),
                UriKind.RelativeOrAbsolute)));
            garbageBrush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "garbage.png"),
                UriKind.RelativeOrAbsolute)));
            bulletfishBrush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "bulletfish.png"),
                UriKind.RelativeOrAbsolute)));
            hpfishBrush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "hpfish.png"),
                UriKind.RelativeOrAbsolute)));
            bgBrush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "bg.bmp"),
                UriKind.RelativeOrAbsolute)));
            sealBrush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "foka.png"),
                UriKind.RelativeOrAbsolute)));
            bulletBrush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "bullet.png"),
                UriKind.RelativeOrAbsolute)));

            penguinBrush = new ImageBrush(new BitmapImage
                (new Uri(Path.Combine("Images", "penguin3.png"),
            UriKind.RelativeOrAbsolute)));

            this.model = model;
            this.lives = model.Lives;
            this.model.LifeLost += (sender, args) => this.InvalidateVisual();
            this.model.Changed+=(sender,args) => this.InvalidateVisual();
            this.model.GamePaused += (sender, args) => this.InvalidateVisual();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (model != null && size.Width > 50 && size.Height > 50)
            {
                double rectWidth = size.Width / model.GameMatrix.GetLength(1);
                double rectHeight = size.Height / model.GameMatrix.GetLength(0);

                drawingContext.DrawRectangle(bgBrush, new Pen(Brushes.Black, 0),
                    new Rect(0, 0, size.Width, size.Height));

                for (int i = 0; i < model.GameMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < model.GameMatrix.GetLength(1); j++)
                    {
                        ImageBrush brush = new ImageBrush();
                        switch (model.GameMatrix[i, j])
                        {
                            case JatekElements.player:
                                brush = new ImageBrush(new BitmapImage
                                        (new Uri(Path.Combine("Images", $"penguin{(int)model.Penguin.direction}.png"),
                                         UriKind.RelativeOrAbsolute)));
                                break;
                            case JatekElements.ice:
                                brush = iceBrush;
                                break;
                            case JatekElements.ice1:
                                brush = ice1Brush;
                                break;
                            case JatekElements.ice2:
                                brush = ice2Brush;
                                break;
                            case JatekElements.ice3:
                                brush = ice3Brush;
                                break;
                            case JatekElements.ice4:
                                brush = ice4Brush;
                                break;
                            case JatekElements.ice5:
                                brush = ice5Brush;
                                break;
                            case JatekElements.garbage:
                                brush = garbageBrush;
                                break;
                            case JatekElements.bulletfish:
                                brush = bulletfishBrush;
                                break;
                            case JatekElements.hpfish:
                                brush = hpfishBrush;
                                break;
                            case JatekElements.seal:
                                brush = sealBrush;
                                break;
                            case JatekElements.bullet:
                                brush = bulletBrush;
                                break;
                            default:
                                break;
                        }

                        drawingContext.DrawRectangle(brush
                                    , new Pen(Brushes.Black, 0),
                                    new Rect(j * rectWidth, i * rectHeight, rectWidth, rectHeight)
                                    );
                    }
                }
            }

        }
    }
}
