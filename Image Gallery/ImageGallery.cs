
using C1.C1Pdf;
using C1.Win.C1Tile;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Image_Gallery
{
    public partial class ImageGallery : Form
    {
        DataFetcher datafetch = new DataFetcher();
        List<ImageItem> imagesList;
        int checkedItems = 0;
        SplitContainer splitContainer1 = new SplitContainer();
        TableLayoutPanel tableLayoutPanel1 = new TableLayoutPanel();
        Panel panel1 = new Panel();
        Panel panel2 = new Panel();
        TextBox _searchBox = new TextBox();
        PictureBox _search = new PictureBox();
        PictureBox _exportimage = new PictureBox();
        PictureBox _downloadimage = new PictureBox();
        C1TileControl _imageTileControl = new C1TileControl();
        C1PdfDocument imagePdfDocument = new C1PdfDocument();
        StatusStrip statusStrip1 = new StatusStrip();
        ToolStripProgressBar toolStripProgressBar1 = new ToolStripProgressBar();
        PanelElement panelElement1 = new PanelElement();
        ImageElement imageElement1 = new ImageElement();
        TextElement textElement1 = new TextElement();

        public ImageGallery()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //set form properties
            this.MaximumSize = new Size(810, 810);
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this.Size = new Size(780, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Image Gallery";

            //set properties for the splitcontainer
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Margin = new Padding(2);
            splitContainer1.Orientation = Orientation.Horizontal;
            splitContainer1.SplitterDistance = 40;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.FixedPanel = FixedPanel.Panel1;

            //set properties for TableLayoutPanel
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.Size = new Size(800, 40);
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));

            //add tablelayoutpanel1 to split container
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);

            //set properties for panel1
            panel1.Location = new Point(477, 0);
            panel1.Size = new Size(287, 40);
            panel1.Dock = DockStyle.Fill;
            panel1.Paint += new PaintEventHandler(Panel1_Paint);

            //set properties for search box
            _searchBox.Name = "_searchBox";
            _searchBox.BorderStyle = BorderStyle.None;
            _searchBox.Location = new Point(16, 9);
            _searchBox.Size = new Size(244, 16);
            _searchBox.Text = "Search Image";
            _searchBox.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);

            //add search box to panel1
            panel1.Controls.Add(_searchBox);
            //add panel1 to 2nd col of tablelayoutpanel
            tableLayoutPanel1.Controls.Add(panel1, 1, 0);

            //set properties for panel2 - col 3 of tablelayout
            panel2.Location = new Point(479, 12);
            panel2.Margin = new Padding(2, 12, 45, 12);
            panel2.Size = new Size(40, 16);
            panel2.TabIndex = 1;

            //set properties for _search (pic box for search icon) 
            _search.Name = "_search";
            _search.Dock = DockStyle.Fill;
            _search.Location = new Point(0, 0);
            _search.Margin = new Padding(0);
            _search.Size = new Size(40, 16);
            _search.SizeMode = PictureBoxSizeMode.Zoom;
            _search.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);
            _search.Click += new EventHandler(_search_ClickAsync);
            _search.Image = Properties.Resources.searchicon;

            //add search pic box to panel2
            panel2.Controls.Add(_search);
            //add panel2 to 3rd col of tablelayoutpanel
            tableLayoutPanel1.Controls.Add(panel2, 2, 0);

            //set properties for _exportimage pic box
            _exportimage.Name = "_exportimage";
            _exportimage.Location = new Point(29, 3);
            _exportimage.Size = new Size(135, 28);
            _exportimage.SizeMode = PictureBoxSizeMode.StretchImage;
            _exportimage.TabIndex = 2;
            _exportimage.Visible = false;
            _exportimage.Click += new EventHandler(_exportImage_Click);
            _exportimage.Paint += new PaintEventHandler(_exportImage_Paint);
            _exportimage.Image = Properties.Resources.exportImage;

            //set properties for _download imag to local sys button
            _downloadimage.Name = "downloadimage";
            _downloadimage.Location = new Point(189, 3);
            _downloadimage.Size = new Size(135, 28);
            _downloadimage.SizeMode = PictureBoxSizeMode.StretchImage;
            _downloadimage.TabIndex = 2;
            _downloadimage.Visible = false;
            _downloadimage.Click += new EventHandler(_downloadImage_Click);
            _downloadimage.Paint += new PaintEventHandler(_downloadImage_Paint);
            _downloadimage.Image = Properties.Resources.saveImage;

            //add _exportimage,_downloadimage pic box to panel 2 of split container
            splitContainer1.Panel2.Controls.Add(_exportimage);
            splitContainer1.Panel2.Controls.Add(_downloadimage);

            //create a group for tiles to be added in the future 
            Group collection = new Group();


            //set up panel element for tile template
            panelElement1.Children.Add(imageElement1);
            panelElement1.Children.Add(textElement1);


            //set proerties for _imageTileControl
            _imageTileControl.Name = "_imageTileControl";
            _imageTileControl.AllowRearranging = true;
            _imageTileControl.AllowChecking = true;
            _imageTileControl.Orientation = LayoutOrientation.Vertical;
            _imageTileControl.CellHeight = 78;
            _imageTileControl.CellSpacing = 11;
            _imageTileControl.CellWidth = 78;
            _imageTileControl.Size = new Size(764, 718);
            _imageTileControl.SurfacePadding = new Padding(12, 4, 12, 4);
            _imageTileControl.SwipeDistance = 20;
            _imageTileControl.SwipeRearrangeDistance = 98;
            _imageTileControl.TileChecked += new EventHandler<TileEventArgs>(c1TileControl1_TileChecked);
            _imageTileControl.TileUnchecked += new EventHandler<TileEventArgs>(c1TileControl1_TileUnchecked);
            _imageTileControl.Paint += new PaintEventHandler(c1TileControl1_Paint);
            _imageTileControl.Groups.Add(collection);
            _imageTileControl.DefaultTemplate.Elements.Add(panelElement1);
            //add _imageTileControl to panel 2 of split container
            splitContainer1.Panel2.Controls.Add(_imageTileControl);

            //set properties for status strip
            statusStrip1.Dock = DockStyle.Bottom;
            statusStrip1.Visible = false;
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            statusStrip1.Items.Add(toolStripProgressBar1);

            //add all controls to the form
            this.Controls.Add(splitContainer1);
            this.Controls.Add(statusStrip1);



        }
        //functionality for search button click(search picture box)
        private async void _search_ClickAsync(object sender, EventArgs e)
        {
            statusStrip1.Visible = true;
            imagesList = await
            datafetch.GetImageData(_searchBox.Text);
            AddTiles(imagesList);
            statusStrip1.Visible = false;
        }
        //add tiles containing images as per search query
        private void AddTiles(List<ImageItem> imageList)
        {
            _imageTileControl.Groups[0].Tiles.Clear();
            foreach (var imageitem in imageList)
            {
                Tile tile = new Tile();
                tile.HorizontalSize = 2;
                tile.VerticalSize = 2;
                _imageTileControl.Groups[0].Tiles.Add(tile);
                Image img = Image.FromStream(new
               MemoryStream(imageitem.Base64));
                Template tl = new Template();
                ImageElement ie = new ImageElement();
                ie.ImageLayout = ForeImageLayout.Stretch;
                tl.Elements.Add(ie);
                tile.Template = tl;
                tile.Image = img;
            }
        }
        //make export and save buttons visible upon tile checking
        private void c1TileControl1_TileChecked(object sender, TileEventArgs e)
        {
            checkedItems++;
            _exportimage.Visible = true;
            _downloadimage.Visible = true;
        }
        //make export and save buttons invisible if no tile is checked
        private void c1TileControl1_TileUnchecked(object sender, TileEventArgs e)
        {
            checkedItems--;
            _exportimage.Visible = checkedItems > 0;
            _downloadimage.Visible = checkedItems > 0;
        }
        //functionality for export to pdf button click
        private void _exportImage_Click(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach (Tile tile in _imageTileControl.Groups[0].Tiles)
            {
                if (tile.Checked)
                {
                    images.Add(tile.Image);
                }
            }
            ConvertToPdf(images);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "pdf";
            saveFile.Filter = "PDF files (*.pdf)|*.pdf*";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {

                imagePdfDocument.Save(saveFile.FileName);

            }
        }
        //Convert selected image to PDF
        private void ConvertToPdf(List<Image> images)
        {
            RectangleF rect = imagePdfDocument.PageRectangle;
            bool firstPage = true;
            foreach (var selectedimg in images)
            {
                if (!firstPage)
                {
                    imagePdfDocument.NewPage();
                }
                firstPage = false;
                rect.Inflate(-72, -72);
                imagePdfDocument.DrawImage(selectedimg, rect);
            }
        }
        //functionality for saving images to local filesystem
        private void _downloadImage_Click(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach (Tile tile in _imageTileControl.Groups[0].Tiles)
            {
                if (tile.Checked)
                {
                    images.Add(tile.Image);
                }
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "jpeg";
            saveFile.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Png Image|*.png";


            foreach (Image img in images)
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {

                    img.Save(saveFile.FileName);

                }
            }
        }
        //paint method for search box
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = _searchBox.Bounds;
            r.Inflate(3, 3);
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
        }
        //paint method for tile control component
        private void c1TileControl1_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawLine(p, 0, 43, 800, 43);
        }
        //paint method for export to pdf button
        private void _exportImage_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(_exportimage.Location.X, _exportimage.Location.Y, _exportimage.Width, _exportimage.Height);
            r.X -= 29;
            r.Y -= 3;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
            e.Graphics.DrawLine(p, new Point(0, 43), new
           Point(this.Width, 43));
        }
        //paint method for save image button
        private void _downloadImage_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(_downloadimage.Location.X, _downloadimage.Location.Y, _downloadimage.Width, _downloadimage.Height);
            r.X -= 189;
            r.Y -= 3;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
            e.Graphics.DrawLine(p, new Point(0, 43), new
           Point(this.Width, 43));
        }

    }
}

