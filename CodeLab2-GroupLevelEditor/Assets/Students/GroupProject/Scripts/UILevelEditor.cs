using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UILevelEditor : GridScript
{
    private GameObject selectedQuad; //to save the selected quad(material)
    public GameObject cornerQuad; //to show the selected quad(material) on screen
    
    string[] gridString = new string[]{
        "ww--|-rw-|----|------------",
        "-ww-|-wr-rrrr-|---rrrr-----",
        "-ww-|----r----|------r--www",
        "-ww-|----r----|------r--ww-",
        "-ww-|----rrrrrrrrrrrr--dww-",
        "-wwwwwwww|----|-------dww--",
        "-wwwwwwwwww---|------d-----",
        "----|---www---|-----dd-----",
        "----|---www---|----dd------",
        "--ddd----w--www---dd-------",
        "--drd----wwww-w--dd--------",
        "--drd----|----wwdd---------",
        "--dddddd-|----|wdd---------",
        "----dddddd----|------------",
        "----|---dd----|------------",
    };

    void Start () {
        gridWidth = gridString[0].Length;
        gridHeight = gridString.Length;
        
        //generate a grid before the princess start, for player to edit
        GetGrid();
    }
	
    protected override Material GetMaterial(int x, int y){

        char c = gridString[y].ToCharArray()[x];

        Material mat;

        switch(c){
            case 'd': 
                mat = mats[1];
                break;
            case 'w': 
                mat = mats[2];
                break;
            case 'r': 
                mat = mats[3];
                break;
            default: 
                mat = mats[0];
                break;
        }
	
        return mat;
    }
    
    void Update()
    {
        //left click to call the function that would select a quad(material)
        if (Input.GetMouseButtonDown(0)) 
        {
            LeftClick();
        }

        //right click to call the function that would replace a quad in grid by your selected one
        if (Input.GetMouseButtonDown(1)) 
        {
            RightClick();
        }
    }

    private void LeftClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedQuad = hit.collider.gameObject;

            //save the clicked quad as the selected quad
            selectedQuad = clickedQuad;
            Debug.Log("Selected Quad: " + selectedQuad.name);
            
            //generate a new quad in corner as the UI indicator
            GenerateQuadInCorner();
        }
    }

    private void RightClick()
    {
        if (selectedQuad != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedQuad = hit.collider.gameObject;

                // replace a quad in grid with the selected quad
                clickedQuad.GetComponent<MeshRenderer>().sharedMaterial = selectedQuad.GetComponent<MeshRenderer>().sharedMaterial;
                Debug.Log("Replaced Quad: " + clickedQuad.name + " with " + selectedQuad.name);
            }
        }
    }
    
    private void GenerateQuadInCorner()
    {
        // destroy the old quad in corner
        if (cornerQuad != null)
        {
            Destroy(cornerQuad);
        }

        // generate a new quad at a specific position as the UI indicator, to show what quad(material) you have selected
        Vector3 cornerPosition = new Vector3(-81, 53, 0);
        cornerQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        cornerQuad.transform.localScale = new Vector3(selectedQuad.transform.localScale.x, selectedQuad.transform.localScale.y, selectedQuad.transform.localScale.z);
        cornerQuad.transform.position = cornerPosition;
        cornerQuad.GetComponent<MeshRenderer>().sharedMaterial = selectedQuad.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
