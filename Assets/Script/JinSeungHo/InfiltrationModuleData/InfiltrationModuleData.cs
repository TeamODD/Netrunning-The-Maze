using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewInfiltrationModuleData", menuName = "Infiltration/Module Data", order = 0)]
public class InfiltrationModuleData : ScriptableObject
{
    [Header("ID"), SerializeField]
    private int _ID;

    [Header("이름"), SerializeField]
    private string _name;

    [Header("설명"), SerializeField]
    private string _description;

    [Header("아이콘"), SerializeField]
    private Image _icon;

    [Header("최대 스택"), SerializeField]
    private int _maxStack;

    [Header("등장 가중치"), SerializeField]
    private float _spawnWeight;

    [Header("효과 종류"), SerializeField]
    private ModuleEffectType _moduleEffect;

    [Header("스택 당 수치"), SerializeField]
    private float _statPerStack;

    [Header("중복 가능 여부"), SerializeField]
    private bool _isStackable;
}
