using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class bojoing : PhysicsGame
{
    const double nopeus = 200;
    const double hyppyNopeus = 750;
    const int RUUDUN_KOKO = 40;

    PlatformCharacter pelaaja1;

    Image pelaajanKuva1 = LoadImage("ihminen (peliin)");
    Image pelaajanKuva2 = LoadImage("ihminen 2 (peliin)");
    Image tahtiKuva = LoadImage("tahti");

    SoundEffect maaliAani = LoadSoundEffect("maali");
    private Vector pelaajanPaikka;
    private double pelaajanLeveys;
    private double pelaajanKorkeus;

    public override void Begin()
    {
        Gravity = new Vector(0, -1000);

        LuoKentta();
        //LisaaNappaimet();

        MultiSelectWindow alkuValikko = new MultiSelectWindow("pelaaja valikko",
        "XD LOL", "sumopainia");
        Add(alkuValikko);
        alkuValikko.AddItemHandler(0, LisaaPelaajaKuvasta, pelaajanKuva1);
        alkuValikko.AddItemHandler(1, LisaaPelaajaKuvasta, pelaajanKuva2);

        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
    }

    void LuoKentta()
    {
        ColorTileMap kentta = ColorTileMap.FromLevelAsset("kentta");
        kentta.SetTileMethod(Color.Black, LisaaTaso);
        kentta.SetTileMethod(Color.FromPaintDotNet(0, 4), LisaaTahti);
        kentta.SetTileMethod(Color.FromPaintDotNet(0, 2), LisaaPelaaja);
        kentta.SetTileMethod(Color.FromPaintDotNet(0, 10), LisaaVesi);
        kentta.SetTileMethod(Color.FromPaintDotNet(1, 1), LisaaPiikit);
        kentta.Execute(RUUDUN_KOKO, RUUDUN_KOKO);
        Level.CreateBorders();
        Level.Background.CreateGradient(Color.White, Color.SkyBlue);
    }

    void LisaaTaso(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = Color.Green;
        taso.CollisionIgnoreGroup = 1;
        Add(taso);
    }

    void LisaaTahti(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject tahti = PhysicsObject.CreateStaticObject(leveys, korkeus);
        tahti.IgnoresCollisionResponse = true;
        tahti.Position = paikka;
        tahti.Image = tahtiKuva;
        tahti.Tag = "tahti";
        tahti.CollisionIgnoreGroup = 1;
        Add(tahti);
    }

    void LisaaVesi(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject vesi = PhysicsObject.CreateStaticObject(leveys, korkeus);
        vesi.Position = paikka;
        Color omaVari = Color.FromHexCode( Color.Blue.ToString() );
        omaVari.AlphaComponent = 124;
        vesi.Color = omaVari;
        vesi.IgnoresCollisionResponse = true;
        vesi.CollisionIgnoreGroup = 1;
        AddCollisionHandler(vesi, "pelaaja", Kelluu);
        Add(vesi, 1);
    }
    void LisaaPiikit(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject piikki = PhysicsObject.CreateStaticObject(leveys, korkeus);
        piikki.Position = paikka;
        piikki.Color = Color.Gray;
        piikki.Shape = Shape.Triangle;
        piikki.CollisionIgnoreGroup = 1;
        Add(piikki);

        AddCollisionHandler(piikki, "pelaaja", kuolee);
    }
    void kuolee(PhysicsObject kuka, PhysicsObject kehen) {
        pelaaja1.Position = pelaajanPaikka;
    }

    void LisaaPelaajaKuvasta(Image pelaajanKuva)
    {
        pelaaja1 = new PlatformCharacter(pelaajanLeveys, pelaajanKorkeus);
        pelaaja1.Tag = "pelaaja";
        pelaaja1.Position = pelaajanPaikka;
        pelaaja1.Mass = 4.0;
        pelaaja1.Image = pelaajanKuva;
        Add(pelaaja1);


        Camera.Follow(pelaaja1);
        LisaaNappaimet();
    }

    void LisaaPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaajanPaikka = paikka;
        pelaajanLeveys = leveys;
        pelaajanKorkeus = korkeus;
    }

    void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

        Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, -nopeus);
        Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, nopeus);
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);

        ControllerOne.Listen(Button.Back, ButtonState.Pressed, Exit, "Poistu pelistä");

        ControllerOne.Listen(Button.DPadLeft, ButtonState.Down, Liikuta, "Pelaaja liikkuu vasemmalle", pelaaja1, -nopeus);
        ControllerOne.Listen(Button.DPadRight, ButtonState.Down, Liikuta, "Pelaaja liikkuu oikealle", pelaaja1, nopeus);
        ControllerOne.Listen(Button.A, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
    }

    void Liikuta(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Walk(nopeus);
    }

    void Hyppaa(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Jump(nopeus);
    }
    void Kelluu(PhysicsObject kuka, PhysicsObject kehen)
    {
        pelaaja1.Hit(new Vector(0, 700));
    }
   
    
}