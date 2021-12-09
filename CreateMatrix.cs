using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CreateMatrix : MonoBehaviour
{
    [SerializeField] private Vector2Int _matrixSize;
    [SerializeField] private float _borderSize;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private InputField _inputFieldPrefab;
    [SerializeField] private InputField _inputFieldPesimism;
    [SerializeField] private Text _valdText;

    private InputField[,] _inputFields;
    private int[,] _numbers;

    private void Start()
    {
        _inputFields = new InputField[_matrixSize.x, _matrixSize.y];
        _numbers = new int[_matrixSize.x, _matrixSize.y];
        //MAX x:29 y:16 size

        for (int i = 0; i < _matrixSize.x; i++)
        {
            for (int j = 0; j < _matrixSize.y; j++)
            {
                _inputFields[i, j] = Instantiate(_inputFieldPrefab, _parent);
                _inputFields[i, j].transform.position =
                    new Vector2(64, 64) + new Vector2(i * _borderSize, j * _borderSize);
            }
        }
    }

    public void Calculate()
    {
        for (int i = 0; i < _matrixSize.x; i++)
        {
            for (int j = 0; j < _matrixSize.y; j++)
            {
                if (_inputFields[i, j].text == "" || Regex.IsMatch(_inputFields[i, j].text, @"^[a-zA-Z]+$"))
                {
                    Debug.LogError("Must be numbers");
                    return;
                }

                _numbers[i, j] = Convert.ToInt32(_inputFields[i, j].text);
            }
        }


        Valda();
        Gurvic();
    }

    private void Valda()
    {
        int[] checkNumber = new int[_matrixSize.y];

        for (int i = 0; i < checkNumber.Length; i++)
        {
            checkNumber[i] = Int32.MaxValue;
        }

        for (int i = _matrixSize.y - 1; i >= 0; i--)
        {
            for (int j = 0; j < _matrixSize.x; j++)
            {
                if (_numbers[j, i] == 0)
                    continue;

                if (checkNumber[i] > _numbers[j, i])
                {
                    checkNumber[i] = _numbers[j, i];
                }
            }
        }

        for (var index = 0; index < checkNumber.Length; index++)
        {
            if (checkNumber[index] == checkNumber.Min())
            {
                _valdText.text = $"Вальда: A{checkNumber.Length - index}";
            }
        }
    }

    private void Gurvic()
    {
        var pesimizm = Convert.ToDouble(_inputFieldPesimism.text);
        var max = int.MaxValue;
        var h1 = pesimizm * 1d + (1d - pesimizm) * max;
        Debug.Log(h1);
    }
}