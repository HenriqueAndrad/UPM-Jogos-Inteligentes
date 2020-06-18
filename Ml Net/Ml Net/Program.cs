using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Globalization;

namespace Ml_Net
{
    class Program
    {
        static readonly string _testPath = Path.Combine(Environment.CurrentDirectory, "Data", "irisTest.txt");
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "irisTrain.txt");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "IrisClusteringModel.zip");

        static void Main(string[] args)
        {
            var mlContext = new MLContext(seed: 0);
            IDataView dataView = mlContext.Data.LoadFromTextFile<IrisData>(_dataPath, hasHeader: false, separatorChar: ',');
            string featuresColumnName = "Features";
            var pipeline = mlContext.Transforms
                .Concatenate(featuresColumnName, "SepalLength", "SepalWidth", "PetalLength", "PetalWidth")
                .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: 3));
            var model = pipeline.Fit(dataView);
            using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(model, dataView.Schema, fileStream);
            }
            
            var predictor = mlContext.Model.CreatePredictionEngine<IrisData, ClusterPrediction>(model);

            NumberFormatInfo numero = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            numero.NumberDecimalSeparator = ".";

            string[] gravarLinhas = File.ReadAllLines(_testPath);
            for (int i = 0; i < gravarLinhas.Length; i++)
            {
                string[] linha = gravarLinhas[i].Split(",");
                float[] valores = new float[linha.Length - 1];

                for (int j = 0; j < linha.Length - 1; j++)
                {
                    valores[j] = float.Parse(linha[j], numero);
                }
                TestIrisData.Flor = new IrisData { SepalLength = valores[0], SepalWidth = valores[1], PetalLength = valores[2], PetalWidth = valores[3] };
                var prediction = predictor.Predict(TestIrisData.Flor);
                Console.WriteLine($"Cluster: {NomeFlor(prediction.PredictedClusterId)}");
                Console.WriteLine($"Distances: {string.Join(" ", prediction.Distances)}");
            }

            Console.ReadKey();
        }

        static string NomeFlor(uint id)
        {
            if (id == 1)
            {
                return "iris setosa";
            }
            else if (id == 2)
            {
                return "iris versicolor";
            }
            else
            {
                return "iris virginica";
            }
        } 
    }
}
