using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// �l�b�g���[�N�p
using System.Net;
using UnityEngine.Networking;

public class RankingManager : MonoBehaviour
{
    /*Http�ʐM*/
    // hostURL
    //const string _hostPass = @"http://localhost/SampleRanking/";
    // �t�@�C���p�X
    const string _getPass = @"GetRanking.php";
    const string _postPass = @"PostRanking.php";
    const string _createPass = @"CreateTable.php";
    const string _resetPass = @"ResetDB.php";

    /*uGUI*/
    // hostURL
    InputField _hostUrlField;

    // �e�[�u���̍쐬
    InputField _createTableFeild;
    InputField _colum1TableFeild;
    InputField _colum2TableFeild;
    InputField _colum3TableFeild;

    // �e�[�u���̎�M
    Text _rankingLabel;
    InputField _getTableField;

    // �e�[�u���̍X�V
    InputField _upTableField;
    InputField _nameField;
    InputField _scoreField;

    // Start is called before the first frame update
    void Start()
    {
        /*uGUI*/
        // hostURL
        _hostUrlField = GameObject.Find("ServerUrlField").GetComponent < InputField>();
        // �e�[�u���̍쐬
        _createTableFeild = GameObject.Find("CreateTableField").GetComponent<InputField>(); 
        _colum1TableFeild = GameObject.Find("CreateColum1Field").GetComponent<InputField>(); 
        _colum2TableFeild = GameObject.Find("CreateColum2Field").GetComponent<InputField>(); 
        _colum3TableFeild = GameObject.Find("CreateColum3Field").GetComponent<InputField>(); 
        // �e�[�u���̎�M
        _rankingLabel = GameObject.Find("TextRanking").GetComponent<Text>();
        _getTableField = GameObject.Find("GetRankingTableField").GetComponent<InputField>();
        // �e�[�u���̍X�V
        _upTableField = GameObject.Find("UpdateTableField").GetComponent<InputField>();
        _nameField = GameObject.Find("NameTableField").GetComponent<InputField>();
        _scoreField = GameObject.Find("ScoreTableField").GetComponent<InputField>();

        /*input������*/
        _createTableFeild.text = @"Ranking";
        _colum1TableFeild.text = @"rank";
        _colum2TableFeild.text = @"name";
        _colum3TableFeild.text = @"score";
        _getTableField.text = @"Ranking";
        _hostUrlField.text =  @"http://localhost/SampleRanking/";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*�{�^�����\�b�h*/
    public void GetRanking()//��M
    {
        //Unity��http�ʐM���s�����߂ɂ͔񓯊��ʐM���K�v
        //���̂���StartCoroutine���\�b�h���g���āA�ʃX���b�h�Ƀ��\�b�h�����s������
        StartCoroutine(GetRequest(_hostUrlField.text, _getPass));
    }
    public void UpdateRanking()// �����L���O�̍X�V
    {
        //StartCoroutine();
        StartCoroutine(UpdateRanking(_hostUrlField.text, _postPass));
    }
    public void CreateTable()// �e�[�u���̍쐬
    {
        // �񓯊��ʐM������A���̂��ߕʃX���b�h�Ƀ��\�b�h�����s������
        StartCoroutine(PostCreate(_hostUrlField.text, _createPass));
    
    }
    public void ResetDB() // DB�̏�����
    {
        StartCoroutine(GetRequest(_hostUrlField.text, _resetPass));
    }

    /*Http���\�b�h*/
    //�ʃX���b�h�ŏ�����������(�R���[�`���Ƃ��ď�����������)���߂ɂ�  
    //IEnumerator��Ԃ�l�Ƃ��Ď��K�v������܂��B
    //����͕ʃX���b�h���猋�ʂ��Ԃ��Ă���܂Ŕ񓯊��ɉ��x�����ʂ�v������K�v�����邽�߂ł��B
    IEnumerator GetRequest(string serverUrl, string getPass)
    {
        /*URL�̍쐬*/
        string _getUrl = serverUrl + getPass;
        _getUrl +=  @"?" + @"table=" + _getTableField.text;


        /*GetRequest*/
        //using�ɂ��X�R�[�v�𔲂����玩���Ń��������
        using UnityWebRequest _getRequest = UnityWebRequest.Get(_getUrl);

        /*yield return*/
        yield return _getRequest.SendWebRequest();

        //�ʐM�̃G���[����
        if (_getRequest.isNetworkError)
        {
            // �G���[���N�����ꍇ�̓G���[���e��\��
            Debug.Log(_getRequest.error);
            Debug.Log(_getUrl);
        }
        else
        {
            //�����L���O�e�L�X�g�̍X�V
            Debug.Log(_getUrl);
            _rankingLabel.text = _getRequest.downloadHandler.text;
        }
    }
    IEnumerator UpdateRanking(string hostUrl,string postPass)
    {
        /*URL�̍쐬*/
        string _postUrl = hostUrl + postPass;

        /*�t�H�[�����쐬*/
        WWWForm form = new WWWForm();
        form.AddField("table", _upTableField.text);
        form.AddField("name", _nameField.text);
        form.AddField("score", _scoreField.text);

        /*PostRequest*/
        using UnityWebRequest _postRequest = UnityWebRequest.Post(_postUrl, form);

        /*yiled return*/
        yield return _postRequest.SendWebRequest();

        /*�G���[����*/
        if (_postRequest.isNetworkError)
        {
            // �ʐM���s
            Debug.Log(_postRequest);
            Debug.Log(_postUrl);
        }
        else
        {
            // �ʐM����
            Debug.Log(_postRequest.downloadHandler.text);
        }
    }
    // �R���[�`���ŏ��������邽�߂ɂ́AIEnumerator�𔃂���n�Ƃ��Ď����\�b�h
    IEnumerator PostCreate(string hostUrl,string filePass)
    {
        /*URL�쐬*/
        string _postUrl = hostUrl + filePass;

        /*�t�H�[���̍쐬*/
        //�|�X�g��URL�݂̂̒ʐM�ł͂Ȃ��AURL�ɑ債��From��n�����ƂŒʐM����B
        //�Ȃ̂Ńt�H�[�����쐬����K�v������B
        WWWForm form = new WWWForm();
        // �t�H�[���Ƀf�[�^��}��
        form.AddField("table",_createTableFeild.text); 
        form.AddField("colum1",_colum1TableFeild.text); 
        form.AddField("colum2",_colum2TableFeild.text); 
        form.AddField("colum3",_colum3TableFeild.text);

        /*Post Request*/
        // using => �����I�Ƀ������J��
        using UnityWebRequest _postRequest = UnityWebRequest.Post(_postUrl, form);
        
        /*yiled return*/
        yield return _postRequest.SendWebRequest();

        /*�G���[����*/
        if(_postRequest.isNetworkError)
        {
            // �ʐM���s
            Debug.Log(_postRequest);
            Debug.Log(_postUrl);
        }
        else
        {
            // �ʐM����
            Debug.Log(_postRequest.downloadHandler.text);
        }
    }
}
