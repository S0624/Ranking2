using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ネットワーク用
using System.Net;
using UnityEngine.Networking;

public class RankingManager : MonoBehaviour
{
    /*Http通信*/
    // hostURL
    //const string _hostPass = @"http://localhost/SampleRanking/";
    // ファイルパス
    const string _getPass = @"GetRanking.php";
    const string _postPass = @"PostRanking.php";
    const string _createPass = @"CreateTable.php";
    const string _resetPass = @"ResetDB.php";

    /*uGUI*/
    // hostURL
    InputField _hostUrlField;

    // テーブルの作成
    InputField _createTableFeild;
    InputField _colum1TableFeild;
    InputField _colum2TableFeild;
    InputField _colum3TableFeild;

    // テーブルの受信
    Text _rankingLabel;
    InputField _getTableField;

    // テーブルの更新
    InputField _upTableField;
    InputField _nameField;
    InputField _scoreField;

    // Start is called before the first frame update
    void Start()
    {
        /*uGUI*/
        // hostURL
        _hostUrlField = GameObject.Find("ServerUrlField").GetComponent < InputField>();
        // テーブルの作成
        _createTableFeild = GameObject.Find("CreateTableField").GetComponent<InputField>(); 
        _colum1TableFeild = GameObject.Find("CreateColum1Field").GetComponent<InputField>(); 
        _colum2TableFeild = GameObject.Find("CreateColum2Field").GetComponent<InputField>(); 
        _colum3TableFeild = GameObject.Find("CreateColum3Field").GetComponent<InputField>(); 
        // テーブルの受信
        _rankingLabel = GameObject.Find("TextRanking").GetComponent<Text>();
        _getTableField = GameObject.Find("GetRankingTableField").GetComponent<InputField>();
        // テーブルの更新
        _upTableField = GameObject.Find("UpdateTableField").GetComponent<InputField>();
        _nameField = GameObject.Find("NameTableField").GetComponent<InputField>();
        _scoreField = GameObject.Find("ScoreTableField").GetComponent<InputField>();

        /*input初期化*/
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

    /*ボタンメソッド*/
    public void GetRanking()//受信
    {
        //Unityでhttp通信を行うためには非同期通信が必要
        //そのためStartCoroutineメソッドを使って、別スレッドにメソッドを実行させる
        StartCoroutine(GetRequest(_hostUrlField.text, _getPass));
    }
    public void UpdateRanking()// ランキングの更新
    {
        //StartCoroutine();
        StartCoroutine(UpdateRanking(_hostUrlField.text, _postPass));
    }
    public void CreateTable()// テーブルの作成
    {
        // 非同期通信がいる、そのため別スレッドにメソッドを実行させる
        StartCoroutine(PostCreate(_hostUrlField.text, _createPass));
    
    }
    public void ResetDB() // DBの初期化
    {
        StartCoroutine(GetRequest(_hostUrlField.text, _resetPass));
    }

    /*Httpメソッド*/
    //別スレッドで処理をさせる(コルーチンとして処理をさせる)ためには  
    //IEnumeratorを返り値として持つ必要があります。
    //これは別スレッドから結果が返ってくるまで非同期に何度も結果を要求する必要があるためです。
    IEnumerator GetRequest(string serverUrl, string getPass)
    {
        /*URLの作成*/
        string _getUrl = serverUrl + getPass;
        _getUrl +=  @"?" + @"table=" + _getTableField.text;


        /*GetRequest*/
        //usingによりスコープを抜けたら自動でメモリ解放
        using UnityWebRequest _getRequest = UnityWebRequest.Get(_getUrl);

        /*yield return*/
        yield return _getRequest.SendWebRequest();

        //通信のエラー処理
        if (_getRequest.isNetworkError)
        {
            // エラーが起きた場合はエラー内容を表示
            Debug.Log(_getRequest.error);
            Debug.Log(_getUrl);
        }
        else
        {
            //ランキングテキストの更新
            Debug.Log(_getUrl);
            _rankingLabel.text = _getRequest.downloadHandler.text;
        }
    }
    IEnumerator UpdateRanking(string hostUrl,string postPass)
    {
        /*URLの作成*/
        string _postUrl = hostUrl + postPass;

        /*フォームを作成*/
        WWWForm form = new WWWForm();
        form.AddField("table", _upTableField.text);
        form.AddField("name", _nameField.text);
        form.AddField("score", _scoreField.text);

        /*PostRequest*/
        using UnityWebRequest _postRequest = UnityWebRequest.Post(_postUrl, form);

        /*yiled return*/
        yield return _postRequest.SendWebRequest();

        /*エラー処理*/
        if (_postRequest.isNetworkError)
        {
            // 通信失敗
            Debug.Log(_postRequest);
            Debug.Log(_postUrl);
        }
        else
        {
            // 通信成功
            Debug.Log(_postRequest.downloadHandler.text);
        }
    }
    // コルーチンで処理させるためには、IEnumeratorを買えり地として持つメソッド
    IEnumerator PostCreate(string hostUrl,string filePass)
    {
        /*URL作成*/
        string _postUrl = hostUrl + filePass;

        /*フォームの作成*/
        //ポストはURLのみの通信ではなく、URLに大してFromを渡すことで通信する。
        //なのでフォームを作成する必要がある。
        WWWForm form = new WWWForm();
        // フォームにデータを挿入
        form.AddField("table",_createTableFeild.text); 
        form.AddField("colum1",_colum1TableFeild.text); 
        form.AddField("colum2",_colum2TableFeild.text); 
        form.AddField("colum3",_colum3TableFeild.text);

        /*Post Request*/
        // using => 自動的にメモリ開放
        using UnityWebRequest _postRequest = UnityWebRequest.Post(_postUrl, form);
        
        /*yiled return*/
        yield return _postRequest.SendWebRequest();

        /*エラー処理*/
        if(_postRequest.isNetworkError)
        {
            // 通信失敗
            Debug.Log(_postRequest);
            Debug.Log(_postUrl);
        }
        else
        {
            // 通信成功
            Debug.Log(_postRequest.downloadHandler.text);
        }
    }
}
