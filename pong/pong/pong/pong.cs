using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class pong : PhysicsGame
{
    Vector nopeusYlos = new Vector(0, 200);
    Vector nopeusAlas = new Vector(0, -200);

    PhysicsObject pallo;

    PhysicsObject maila1;
    PhysicsObject maila2;

    public override void Begin()
    {
        LuoKentta();
        AsetaOhjelma();
        AloitaPeli();
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        
    }
    void LuoKentta()
    {
        pallo = new PhysicsObject(60.0, 60.0);
        pallo.Shape = Shape.Circle;
        pallo.X = -200.0;
        pallo.Y = 0.0;
        pallo.Restitution = 1.0;
        Add(pallo);

       maila1 = LuoMaila(Level.Left + 20.0, 0.0);
       maila2 = LuoMaila(Level.Right - 20.0, 0.0);


        Level.CreateBorders(1.0, false);


        Level.Background.Color = Color.Black;

        Camera.ZoomToLevel();
    }
    void AloitaPeli()
    {
        Vector impulssi = new Vector(500.0, 0.0);
        pallo.Hit(impulssi);
    }


    PhysicsObject LuoMaila(double x, double y)
    {

        PhysicsObject maila = PhysicsObject.CreateStaticObject(20.0, 100.0);
        maila.Shape = Shape.Rectangle;
        maila.X = x;
        maila.Y = y;
        maila.Restitution = 1.0;
        Add(maila);
        return maila;
    }
    void AsetaOhjelma()
    {
        Keyboard.Listen(Key.A,        ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa ylös", maila1, nopeusYlos);
        Keyboard.Listen(Key.A,        ButtonState.Released,AsetaNopeus, null, maila1, Vector.Zero);
        Keyboard.Listen(Key.Z,        ButtonState.Down, AsetaNopeus, "Pelaaja 1: Liikuta mailaa alas", maila1, nopeusAlas);
        Keyboard.Listen(Key.Z,        ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);

        Keyboard.Listen(Key.Up, ButtonState.Down, AsetaNopeus, "Pelaaja 2: Liikuta mailaa ylös", maila2, nopeusYlos);
        Keyboard.Listen(Key.Up, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);
        Keyboard.Listen(Key.Down, ButtonState.Down,   AsetaNopeus,"Pelaaja2: Liikuta mailaa alas", maila2, nopeusAlas);
        Keyboard.Listen(Key.Down, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);

        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjelma");
        Keyboard.Listen(Key.Escape,   ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    void AsetaNopeus(IPhysicsObject maila, Vector nopeus)
{   
      if ((nopeus.Y < 0) && (maila.Bottom < Level.Bottom))
      {
          maila.Velocity = Vector.Zero;
          return;
        
        if ((nopeus.Y > 0) &&  (maila.Top > Level.Top))
        {
            maila.Velocity = Vector.Zero;
            return;
        }


        maila.Velocity = nopeus;
    }
}




