using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class pepepoopoo : MonoBehaviour
{
    public Sprite pepeTheFroh;
    public Tilemap cakedUp;
    public Tile LeftWall;
    public Tile RightWall;
    public Tile Floor;
    public Tile Ceiling;
    public Tile Black;
    // Start is called before the first frame update
    private void OnDrawGizmos() {
            var pixelMap = pepeTheFroh.texture.GetPixels();
        
        //given a pixel, if it isnt a white pixel, place a tile at that position
        for (int i = 0; i < pepeTheFroh.texture.width; i++) {
            for (int j = 0; j < pepeTheFroh.texture.height; j++) {
                if (pixelMap[i + j * pepeTheFroh.texture.width] != Color.white) {
                    // depending on the surrounding pixels, place a tile
                    try{
if (pixelMap[i + (j+1) * pepeTheFroh.texture.width] == Color.white) {
                        cakedUp.SetTile(new Vector3Int(i, j, 0), Floor);
                    }
                    else if (pixelMap[i + (j-1) * pepeTheFroh.texture.width] == Color.white) {
                        cakedUp.SetTile(new Vector3Int(i, j, 0), Ceiling);
                    }
                    else if (pixelMap[(i+1) + j * pepeTheFroh.texture.width] == Color.white) {
                        cakedUp.SetTile(new Vector3Int(i, j, 0), LeftWall);
                    }
                    else if (pixelMap[(i-1) + j * pepeTheFroh.texture.width] == Color.white) {
                        cakedUp.SetTile(new Vector3Int(i, j, 0), RightWall);
                    }
                    else {
                        cakedUp.SetTile(new Vector3Int(i, j, 0), Black);
                    }
                    }
                    catch
                    {
                        cakedUp.SetTile(new Vector3Int(i, j, 0), Black);
                    }
                }
            }
        }

        // Debug.Log(pepeTheFroh.texture.GetPixel(0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
