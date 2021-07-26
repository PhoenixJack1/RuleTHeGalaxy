using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Client
{
    public class GSPlanet
    {
        public int ID;
        public GSString Name;
        public int StarID;
        public GSStar Star;
        public int Orbit;
        public int Size;
        public int Moons;
        public bool HasBelt;
        public int ImageType;
        public PlanetTypes PlanetType;
        public PlanetSide PlanetSide = PlanetSide.Unknown;
        public byte LandOrAvanpost; //0 - колония, 1 - аванпост
        public int LocationID = -1;
        public bool Pillage;
        public bool Conquer;
        public byte RiotIndicator;
        public int LandsCount;
        public int MaxPopulation;
        public int Locations;
        public RareBuildings RareBuilding;
        public bool QuestPlanet = false;
        public int LandBackgroundID = 0;
        public Canvas InGalaxyCanvas;
        public Mission2 PlanetMission = null;
        public GSPlanet(int id, GSString name, int starID, int orbit, int size, int moons, bool hasbelt, int imageType, PlanetTypes type, int population, int locations, RareBuildings rare)
        {
            ID = id;
            Name = name;
            StarID = starID;
            Star = Links.Stars[StarID];
            Orbit = orbit;
            Size = size;
            Moons = moons;
            HasBelt = hasbelt;
            ImageType = imageType;
            LandsCount = 1;
            PlanetType = type;
            MaxPopulation = population;
            Locations = locations;
            RareBuilding = rare;
            switch (type)
            {
                case PlanetTypes.Burned: LandBackgroundID = 1; break;
                case PlanetTypes.Freezed: LandBackgroundID = 2; break;
                default: LandBackgroundID = 0; break;
            }
            Links.Stars[StarID].Planets.Add(Orbit, this);
        }
        public PlanetPanelInfo PanelInfo {get { return new PlanetPanelInfo(this, false); } }
        public override string ToString()
        {
            return String.Format("{0} {1} {2}", Star.Name, Name, PlanetSide.ToString());
        }
        class InStellarPlanetSizes
        {
            public double OrbitRadius;
            public double OrbitLocation;
            public double PlanetRadius;
            public double BeltWidth;
            public double BeltHeight;
            public double Degree;
            public InStellarPlanetSizes(GSPlanet planet)
            {
                OrbitRadius = 4 + planet.Orbit * 0.6;
                OrbitLocation = 10 - OrbitRadius;
                PlanetRadius = (planet.Size + 2) * 0.06;
                BeltWidth = PlanetRadius * 2 * 1.4;
                BeltHeight = PlanetRadius * 2 * 0.6;
                Degree= (GSGameInfo.ServerTime.DayOfYear + planet.Orbit * 36) % 360;
            }
        }
        public InStellarPlanetShapes GetPlanetCanvas2()
        {
            GSStar star = Links.Stars[StarID];
            InStellarPlanetShapes shapes = new InStellarPlanetShapes();
            shapes.Planet = this;
            Ellipse orbit = new Ellipse();
            shapes.ID = ID;
            double orbitradius = 4 + Orbit * 0.6;
            orbit.Width = orbitradius * 2;
            orbit.Height = orbitradius * 2;
         
            double OrbitLeft = 10-orbitradius;
            double OrbitTop = 10-orbitradius;
            Canvas.SetLeft(orbit, OrbitLeft);
            Canvas.SetTop(orbit, OrbitTop);
            orbit.StrokeThickness = 0.02;
            orbit.Stroke = OrbitBrush;
            orbit.StrokeDashArray = new DoubleCollection(new double[] { 2, 2 });
            shapes.OrbitEllipse = orbit;
            
            double canvasX = 0; double canvasY = 0;
            Canvas canvas = new Canvas();
            shapes.canvas = canvas;
            canvas.Tag = shapes;
            
            Canvas.SetZIndex(canvas, 50);
            Ellipse planetellipse = new Ellipse();
            double planetradius = (Size + 2) * 0.06;
            
            if (HasBelt)
            {
                double beltwidth = planetradius * 2 * 1.4;
                double beltheight = planetradius * 2 * 0.6;
            }
            canvas.Width = planetradius * 2;
            canvas.Height = planetradius * 2;
            planetellipse.Width = planetradius * 2;
            planetellipse.Height = planetradius * 2;
            planetellipse.Fill = Brushes.Green;
            double degree = (GSGameInfo.ServerTime.DayOfYear + Orbit * 36) % 360;
            canvasX =10+ orbitradius * Math.Cos(Math.PI * degree / 180);
            canvasY = 10+orbitradius * Math.Sin(Math.PI * degree / 180);
            Canvas.SetLeft(canvas, canvasX - planetradius);
            Canvas.SetTop(canvas, canvasY - planetradius);
            canvas.Children.Add(planetellipse);
            planetellipse.Fill = Links.Brushes.PlanetsBrushes[ImageType];
            if (HasBelt)
            {
                Ellipse belt = new Ellipse();
                belt.Width = planetradius * 2 * 1.4;
                belt.Height = planetradius * 2 * 0.6;
                belt.Stroke = Brushes.Brown;
                belt.StrokeThickness = 0.1;
                LinearGradientBrush beltoppacitymask = new LinearGradientBrush(Colors.White, Color.FromArgb(0, 0, 0, 0), new Point(0.5, 1), new Point(0.5, 0));
                beltoppacitymask.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.7));
                belt.OpacityMask = beltoppacitymask;
                double BeltLeft =- belt.Width * 0.15;
                double BeltTop = belt.Height / 2;
                Canvas.SetLeft(belt, BeltLeft);
                Canvas.SetTop(belt, BeltTop);
                canvas.Children.Add(belt);
                Canvas.SetZIndex(belt, 10);

            }
            for (int i = 0; i < Moons; i++)
            {
                Ellipse moonorbit = new Ellipse();
                moonorbit.Width = planetradius * 2 + (i + 1) * 0.2;
                moonorbit.Height = planetradius * 2 + (i + 1) * 0.2;
                double MoonOrbitLeft = planetradius - moonorbit.Width / 2;
                double MoonOrbitTop = planetradius - moonorbit.Height / 2;
                Canvas.SetLeft(moonorbit, MoonOrbitLeft);
                Canvas.SetTop(moonorbit, MoonOrbitTop);
                moonorbit.Stroke = OrbitBrush;
                moonorbit.StrokeThickness = 0.02;
                canvas.Children.Add(moonorbit);

                Ellipse moon = new Ellipse();
                moon.Width = 0.1;
                moon.Height = 0.1;
                moon.Fill = Links.Brushes.PlanetsBrushes[(StarID + ID + i * Orbit) % Links.Brushes.PlanetsBrushes.Count];
                if (ID == 2) moon.Fill = Links.Brushes.PlanetsBrushes[1];
                double angle = (Math.Abs((double)DateTime.Now.Minute / 60 * 360 + (Orbit + i) % 5 * 90 - 360));
                double MoonLeft = MoonOrbitLeft + moonorbit.Width / 2 + moonorbit.Width / 2 * Math.Cos(Math.PI * angle / 180) - 0.05;
                double MoonTop = MoonOrbitTop + moonorbit.Height / 2 + moonorbit.Height / 2 * Math.Sin(Math.PI * angle / 180) - 0.05;
                Canvas.SetLeft(moon, MoonLeft);
                Canvas.SetTop(moon, MoonTop);
                canvas.Children.Add(moon);

            }
           
            StackPanel DopPanel = new StackPanel(); DopPanel.Orientation = Orientation.Vertical;
            canvas.Children.Add(DopPanel);
            Canvas.SetLeft(DopPanel, 2 * planetradius); Canvas.SetZIndex(DopPanel, 10);
            foreach (GSFleet fleet in GSGameInfo.Fleets.Values)
            {
                if (fleet.FleetBase.Land.Planet == this)
                {
                    DopPanel.Children.Add(new FleetEmblem(fleet.Image, 0.35));
                    shapes.Fleets.Add(fleet);
                }

            }
            return shapes;
        }
      
        public Land GetLand()
        {
            if (LocationID == -1) return null;
            if (GSGameInfo.PlayerLands.ContainsKey(LocationID) == false) return null;
            Land land = GSGameInfo.PlayerLands[LocationID];
            if (land.PlanetID != ID) return null;
            return land;
            //foreach (Land land in GSGameInfo.PlayerLands.Values)
            //    if (land.Planet == this) return land;
            //return null;
        }
        public Avanpost GetAvanpost()
        {
            if (LocationID == -1) return null;
            if (GSGameInfo.PlayerAvanposts.ContainsKey(LocationID) == false) return null;
            Avanpost avanpost = GSGameInfo.PlayerAvanposts[LocationID];
            if (avanpost.PlanetID != ID) return null;
            return avanpost;
        }
        static SolidColorBrush OrbitBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
    }
    
    public class InStellarPlanetShapes
    {
        public Ellipse OrbitEllipse;
        public GSPlanet Planet;
        public Canvas canvas;
        public int ID;
        public Land Land;
        public List<GSFleet> Fleets;
        public InStellarPlanetShapes()
        {
            Fleets = new List<GSFleet>();
        }
    }
}
