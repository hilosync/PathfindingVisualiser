using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;

    Canvas gridCanvas;


    HexCell[] cells;

    void Awake() {
        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[height * width];

		for (int y = 0, i = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, y, i++);
            }
        }
    }

    void CreateCell (int x, int y, int i) {
    Vector2 position;
    position.x = x * 10f;
    position.y = y * 10f;

    HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
    cell.transform.SetParent(transform, false);
    cell.transform.localPosition = position;

    Text label = Instantiate<Text>(cellLabelPrefab);
    label.rectTransform.SetParent(gridCanvas.transform, false);
    label.rectTransform.anchoredPosition = new Vector2(position.x, position.y);
    label.text = x.ToString() + "\n" + y.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
