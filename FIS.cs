using System;
using System.Collections.Generic;
using System.Text;
using AForge.Fuzzy;

namespace FuzzyAGV
{
    // Fuzzy Inference System
    public class FIS
    {
        private InferenceSystem IS;

        // Criando o Sistema de inferência com sua base de dados e de regras
        public FIS()
        {
            Fzz1();
        }

        // Realizando a inferencia baseando-se nas variáveis de entrada
        public void DoInference(double rightDist, double leftDist, double frontDist, out double Angle, out double Speed)
        {
            Angle = 0;
            Speed = 50;

            // setting inputs
            IS.SetInput( "RightDistance", (float)rightDist );
            IS.SetInput("LeftDistance", (float)leftDist);
            IS.SetInput("FrontalDistance", (float)frontDist);

            // evaluating outputs
            try
            {
                Angle = IS.Evaluate("Angle");
            }
            catch (Exception)
            {
                ;
            }
            return;
        }


        // initialization of the Fuzzy Inference System
        void Fzz1()
        {
            // linguistic labels (fuzzy sets) that compose the distances
            FuzzySet fsNear = new FuzzySet("Near", new TrapezoidalFunction(
                15, 50, TrapezoidalFunction.EdgeType.Right));
            FuzzySet fsMedium = new FuzzySet("Medium", new TrapezoidalFunction(
                15, 50, 60, 100));
            FuzzySet fsFar = new FuzzySet("Far", new TrapezoidalFunction(
                60, 100, TrapezoidalFunction.EdgeType.Left));

            // right Distance (Input)
            LinguisticVariable lvRight = new LinguisticVariable("RightDistance", 0, 120);
            lvRight.AddLabel(fsNear);
            lvRight.AddLabel(fsMedium);
            lvRight.AddLabel(fsFar);

            // left Distance (Input)
            LinguisticVariable lvLeft = new LinguisticVariable("LeftDistance", 0, 120);
            lvLeft.AddLabel(fsNear);
            lvLeft.AddLabel(fsMedium);
            lvLeft.AddLabel(fsFar);

            // front Distance (Input)
            LinguisticVariable lvFront = new LinguisticVariable("FrontalDistance", 0, 120);
            lvFront.AddLabel(fsNear);
            lvFront.AddLabel(fsMedium);
            lvFront.AddLabel(fsFar);

            // linguistic labels (fuzzy sets) that compose the angle
            FuzzySet fsVN = new FuzzySet("VeryNegative", new TrapezoidalFunction(
                -40, -35, TrapezoidalFunction.EdgeType.Right));
            FuzzySet fsN = new FuzzySet("Negative", new TrapezoidalFunction(
                -40, -35, -25, -20));
            FuzzySet fsLN = new FuzzySet("LittleNegative", new TrapezoidalFunction(
                -25, -20, -10, -5));
            FuzzySet fsZero = new FuzzySet("Zero", new TrapezoidalFunction(
                -10, 5, 5, 10));
            FuzzySet fsLP = new FuzzySet("LittlePositive", new TrapezoidalFunction(
                5, 10, 20, 25));
            FuzzySet fsP = new FuzzySet("Positive", new TrapezoidalFunction(
                20, 25, 35, 40));
            FuzzySet fsVP = new FuzzySet("VeryPositive", new TrapezoidalFunction(
                35, 40, TrapezoidalFunction.EdgeType.Left));

            // angle
            LinguisticVariable lvAngle = new LinguisticVariable("Angle", -50, 50);
            lvAngle.AddLabel(fsVN);
            lvAngle.AddLabel(fsN);
            lvAngle.AddLabel(fsLN);
            lvAngle.AddLabel(fsZero);
            lvAngle.AddLabel(fsLP);
            lvAngle.AddLabel(fsP);
            lvAngle.AddLabel(fsVP);

            // the database
            Database fuzzyDB = new Database();
            fuzzyDB.AddVariable(lvFront);
            fuzzyDB.AddVariable(lvLeft);
            fuzzyDB.AddVariable(lvRight);
            fuzzyDB.AddVariable(lvAngle);

            // creating the inference system
            IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(1000));

            // going Straight
            IS.NewRule("Rule 1", "IF FrontalDistance IS Far THEN Angle IS Zero");
            // going Straight (if can go anywhere)
            IS.NewRule("Rule 2", "IF FrontalDistance IS Far AND RightDistance IS Far AND " +
                "LeftDistance IS Far THEN Angle IS Zero");
            // near right wall
            IS.NewRule("Rule 3", "IF RightDistance IS Near AND LeftDistance IS Medium " +
                "THEN Angle IS LittleNegative");
            // near left wall
            IS.NewRule("Rule 4", "IF RightDistance IS Medium AND LeftDistance IS Near " +
                "THEN Angle IS LittlePositive");
            // near front wall - room at right
            IS.NewRule("Rule 5", "IF RightDistance IS Far AND FrontalDistance IS Near " +
                "THEN Angle IS Positive");
            // near front wall - room at left
            IS.NewRule("Rule 6", "IF LeftDistance IS Far AND FrontalDistance IS Near " +
                "THEN Angle IS Negative");
            // near front wall - room at both sides - go right
            IS.NewRule("Rule 7", "IF RightDistance IS Far AND LeftDistance IS Far AND " +
                "FrontalDistance IS Near THEN Angle IS Positive");
        }

    }
}
