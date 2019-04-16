
// c# companion script
// SpriteUVToShader.cs -------------------------------------------------------------------------------------------------------------------------------- //
// Save you your project, add to your SpriteRenderer gameObject
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteUVToShader : MonoBehaviour {
    Material material;
 
    SpriteRenderer sr;
    Sprite sprite;
    MaterialPropertyBlock mpb;

    void OnValidate() => update();
    void OnWillRenderObject() => update();
    void OnEnable() => init();
    // void Update() => update();

    void checkNulls()
    {
        if (sr==null)
            sr = GetComponent<SpriteRenderer>();
       
        if(sprite != sr.sprite)
            sprite = sr.sprite;
    }

    void init()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        material = new Material(Shader.Find(GameConstants.PixelPerfectShader));
        sr.material = material;
        update();
    }

    void update(){
        if (sr==null)
            sr = GetComponent<SpriteRenderer>();
       
        if(sprite != sr.sprite){
            sprite = sr.sprite;

            applySpriteUV(sr, sprite, ref mpb, 
                GameConstants.UVPassthroughUV, 
                GameConstants.UVPassthroughPivot, 
                GameConstants.UVPassthroughUVCenter);
            applySpriteTX(sr, sprite, ref mpb, 
                GameConstants.UVPassthroughTextureSize, 
                GameConstants.UVPassthroughPixelSize);
        }
    }
   
    public static void applySpriteUV(Renderer renderer, Sprite toSprite, ref MaterialPropertyBlock mpb,
        string uvProp=null, string pivotProp=null, string uvCenterProp=null){
       
        if(toSprite==null) return;
       
        var scale = new Vector2(
            toSprite.textureRect.width/ toSprite.texture.width,
            toSprite.textureRect.height/toSprite.texture.height);
           
        var offset = new Vector2(
            toSprite.rect.x/toSprite.texture.width,
            toSprite.rect.y/toSprite.texture.height);
       
        Vector4 uvVector = new Vector4(scale.x,scale.y,offset.x,offset.y);
        Vector4 pivotVector = new Vector4(toSprite.pivot.x/toSprite.rect.width,toSprite.pivot.y/toSprite.rect.height);
       
        if(string.IsNullOrEmpty(uvProp))
            uvProp = "_MainTex_ST";
 
        if(mpb==null)
            mpb = new MaterialPropertyBlock();
 
        renderer.GetPropertyBlock(mpb);
        mpb.SetTexture("_MainTex", toSprite.texture);
       
        mpb.SetVector(uvProp, uvVector);
        if(!string.IsNullOrEmpty(pivotProp))
            mpb.SetVector(pivotProp, pivotVector);
       
        if(!string.IsNullOrEmpty(uvCenterProp))
            mpb.SetVector(uvCenterProp, new Vector2(
                Mathf.Lerp(uvVector.z, uvVector.z+uvVector.x, pivotVector.x),
                Mathf.Lerp(uvVector.w, uvVector.w+uvVector.y, pivotVector.y)
            ));

       
        renderer.SetPropertyBlock(mpb);
    }
   
   
    public static void applySpriteTX(Renderer renderer, Sprite toSprite, ref MaterialPropertyBlock mpb,
        string texSizeProp=null, string pixSizeProp=null){
       
        if(toSprite==null || string.IsNullOrEmpty(texSizeProp)) return;

        if(mpb==null)
            mpb = new MaterialPropertyBlock();
 
        renderer.GetPropertyBlock(mpb);
       
        mpb.SetFloat(texSizeProp, toSprite.texture.width);
        if(!string.IsNullOrEmpty(pixSizeProp))
            mpb.SetFloat(pixSizeProp, 1f/toSprite.pixelsPerUnit);

        renderer.SetPropertyBlock(mpb);
    }
}