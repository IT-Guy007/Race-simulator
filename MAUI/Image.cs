using System.Drawing.Imaging;
using Model;

namespace MAUI;
using System.Drawing;
#pragma warning disable CA1416
public static class Images {
    private static Dictionary<string, Bitmap> _images;
    private static int xSize = 0;
    private static int ySize = 0;

    public static Bitmap GetImage(string name) {
        if (_images == null) { _images = new Dictionary<string, Bitmap>();}


        if (!_images.ContainsKey(name)) {
            _images[name] = new Bitmap(name);
        }

        return _images[name].Clone() as Bitmap;
    }

    private static void Clear() { _images = new Dictionary<string, Bitmap>();}

    private static Bitmap emptyImage(int x, int y) {
        Bitmap bmp = new Bitmap(x, y);
        for (int i = 0; i < x; i++) {
            for (int j = 0; j < y; j++) {
                bmp.SetPixel(i, j, Color.Black);
            }
        }
        return bmp;
    }

    private static void startImage(Track track) {
        
    }
}