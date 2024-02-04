using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synthesizer
{
    public partial class Synthesizer : Form
    {
        private const int SAMPLE_RATE = 44100;
        private const short BITS_PER_SAMPLE = 16;

        public Synthesizer()
        {
            InitializeComponent();
        }

        private void Synthesizer_Load(object sender, EventArgs e)
        {

        }

        private void Synthesizer_KeyDown(object sender, KeyEventArgs e)
        {
            short[] wave = new short[SAMPLE_RATE];
            byte[] binarywave = new byte[SAMPLE_RATE * sizeof(short)];
            float frequency = 240f;
            for (int i = 0; i < SAMPLE_RATE; i++)
            {
                wave[i] = Convert.ToInt16(short.MaxValue * Math.Sin(((Math.PI * 2 * frequency) / SAMPLE_RATE) * i));
            }
            Buffer.BlockCopy(wave, 0, binarywave, 0, wave.Length * sizeof(short));
            using (MemoryStream memorystream = new MemoryStream())
            using (BinaryWriter binarywriter = new BinaryWriter(memorystream))
            {
                short blockAlign = BITS_PER_SAMPLE / 8;
                int subChunkTwoSize = SAMPLE_RATE * blockAlign;
                binarywriter.Write(new[] { 'R', 'I', 'F', 'F' });
                binarywriter.Write(36 + subChunkTwoSize);
                binarywriter.Write(new[] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
                binarywriter.Write(16);
                binarywriter.Write((short)1);
                binarywriter.Write((short)1);
                binarywriter.Write(SAMPLE_RATE);
                binarywriter.Write(SAMPLE_RATE * blockAlign);
                binarywriter.Write(blockAlign);
                binarywriter.Write(BITS_PER_SAMPLE);
                binarywriter.Write(new[] { 'd', 'a', 't', 'a' });
                binarywriter.Write(subChunkTwoSize);
                binarywriter.Write(binarywave);
                memorystream.Position = 0;
                new SoundPlayer(memorystream).Play();


            }
        }
    }
}
