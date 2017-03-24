using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SpriteRotate
{
    class Program
    {
        private static readonly byte[] memdmp = File.ReadAllBytes("memdmp.bin");

        static readonly Color[] zxcolors =
        {
            Color.Black, Color.Blue, Color.Red, Color.Fuchsia, Color.Lime, Color.Aqua, Color.Yellow, Color.White
        };

        private const int OOO = 0;
        private const int INV = 1;
        private const int RED = 2;
        private const int GRN = 4;
        private const int GIN = 5;
        private const int WGO = 6;  // White and Green
        private const int WGI = 7;  // White and Green and Inverse
        private const int SP1 = 10; // Special case for train wheels

        private static readonly int[] tile0Mods =
        {
            RED, OOO, OOO, OOO, OOO, OOO, OOO, OOO, WGI, WGI, WGI, WGI, WGI, WGI, WGI, OOO, // 0
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, WGI, WGO, WGI, WGO, WGO, WGI, WGI, OOO, // 1
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // 2
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // 3
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // 4
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // 5
            GRN, RED, GRN, SP1, SP1, GRN, GRN, OOO, OOO, GRN, OOO, GRN, OOO, OOO, OOO, OOO, // 6
            OOO, OOO, OOO, RED, GRN, RED, GRN, OOO, GRN, RED, RED, RED, RED, RED, RED, OOO, // 7
        };
        private static readonly int[] tile1Mods =
        {
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, RED, RED, RED, RED, RED, RED, RED, RED, // 8
            OOO, OOO, OOO, OOO, OOO, OOO, GRN, GRN, RED, RED, RED, RED, RED, RED, RED, RED, // 9
            GRN, GRN, GRN, GRN, OOO, OOO, OOO, OOO, RED, RED, RED, RED, RED, RED, RED, RED, // a
            GRN, GRN, GRN, GRN, GRN, GRN, OOO, OOO, RED, RED, RED, RED, RED, RED, RED, RED, // b
            OOO, OOO, OOO, OOO, GRN, GRN, GRN, GRN, OOO, OOO, RED, RED, RED, RED, RED, RED, // c
            OOO, OOO, OOO, OOO, GRN, OOO, OOO, GRN, OOO, OOO, RED, RED, RED, RED, RED, RED, // d
            GRN, GRN, RED, OOO, OOO, OOO, OOO, OOO, OOO, OOO, RED, RED, RED, RED, RED, RED, // e
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, RED, RED, RED, RED, RED, RED, RED, // f
        };
        private static readonly int[] tile2Mods =
        {
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // 8
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // 9
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // a
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // b
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // c
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // d
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // e
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // f
        };
        private static readonly int[] tile3Mods =
        {
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // b
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // c
            OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, OOO, // d
        };

        static void Main(string[] args)
        {
            Bitmap bmpTiles = new Bitmap(180, 240, PixelFormat.Format32bppArgb);

            DrawTiles(14726, bmpTiles, 128, 10, 10);
            DrawTiles(15880, bmpTiles, 128, 10, 10 + 80 + 2);
            DrawTiles(18186, bmpTiles, 48, 10, 10 + 80 + 2 + 80 + 2);

            bmpTiles.Save("tiles.png");

            //Process.Start("tiles.png");

            FileStream fs = new FileStream("SPRITE.MAC", FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine();
            writer.WriteLine("; START OF TILES.MAC");
            RotateTiles(writer, 14726, 128, 0, tile0Mods);    // 034606
            RotateTiles(writer, 15880, 128, 128, tile1Mods);  // 037010
            RotateTiles(writer, 17034, 128, 128, tile2Mods);  // 041212
            RotateTiles(writer, 18188, 48, 176, tile3Mods);   // 043414
            writer.WriteLine();
            writer.Flush();
        }

        static void DrawTiles(int address, Bitmap bmpTiles, int tilecount, int basex, int basey)
        {
            int x = basex;
            int y = basey;
            for (int addr = address; addr < address + tilecount * 9; addr += 9)
            {
                byte attr = memdmp[addr + 8];
                int ink = attr & 7;
                int paper = (attr >> 3) & 7;
                Color colorink = zxcolors[ink];
                Color colorpaper = zxcolors[paper];

                for (int i = 0; i < 8; i++)
                {
                    byte b = memdmp[addr + i];
                    for (int j = 0; j < 8; j++)
                    {
                        bmpTiles.SetPixel(x + j, y + i, ((b >> (7 - j)) & 1) == 1 ? colorink : colorpaper);
                    }
                }

                x += 10;
                if (x >= basex + 160)
                {
                    y += 10;
                    x = basex;
                }
            }
        }

        public static void RotateTiles(StreamWriter writer, int address, int tilecount, int tileoffset, int[] tileMods)
        {
            writer.WriteLine();
            writer.WriteLine("; Блок из {0} тайлов с {1} тайла", EncodeOctalString((byte)tilecount), EncodeOctalString((byte)tileoffset));
            writer.WriteLine("\t.EVEN");
            string saddress = "Z" + EncodeOctalString2(address - 2).Substring(1);
            writer.WriteLine("{0}::.BYTE\t{2}, {1}", saddress, EncodeOctalString((byte)tilecount), EncodeOctalString((byte)tileoffset));

            for (int tile = 0; tile < tilecount; tile++)
            {
                int addr = address + tile * 9;
                int tilemod = tileMods[tile];
                writer.Write("\t.BYTE\t");
                for (int i = 0; i < 8; i++)
                {
                    byte b = memdmp[addr + i];
                    int bb = 0;
                    for (int j = 0; j < 8; j++)
                        bb |= ((b >> (7 - j)) & 1) << j;
                    byte bb1 = (byte)bb;
                    byte bb2 = (byte)bb;

                    if (tilemod == INV)
                    {
                        bb1 = (byte)~bb1;  bb2 = (byte)~bb2;
                    }
                    else if (tilemod == RED)
                    {
                        bb1 = 0;
                    }
                    else if (tilemod == GRN)
                    {
                        bb2 = 0;
                    }
                    else if (tilemod == GIN)
                    {
                        bb1 = (byte)~bb1;  bb2 = 0;
                    }
                    else if (tilemod == WGO)
                    {
                        bb1 = 0xff;
                    }
                    else if (tilemod == WGI)
                    {
                        bb1 = 0xff;  bb2 = (byte)~bb2;
                    }
                    else if (tilemod == SP1)  // Special case for wheels
                    {
                        if (i < 6) bb2 = 0;
                    }

                    writer.Write(EncodeOctalString(bb1));
                    writer.Write(",");
                    writer.Write(EncodeOctalString(bb2));

                    if (i < 7)
                        writer.Write(",");
                }
                byte attr = memdmp[addr + 8];
                writer.WriteLine("\t; {1}", EncodeOctalString(attr), EncodeOctalString((byte)(tileoffset + (addr - address) / 9)));
            }
        }

        public static string EncodeOctalString(byte value)
        {
            //convert to int, for cleaner syntax below. 
            int x = (int)value;

            return string.Format(
                @"{0}{1}{2}",
                ((x >> 6) & 7),
                ((x >> 3) & 7),
                (x & 7)
            );
        }

        public static string EncodeOctalString2(int x)
        {
            return string.Format(
                @"{0}{1}{2}{3}{4}{5}",
                ((x >> 15) & 7),
                ((x >> 12) & 7),
                ((x >> 9) & 7),
                ((x >> 6) & 7),
                ((x >> 3) & 7),
                (x & 7)
            );
        }
    }
}
