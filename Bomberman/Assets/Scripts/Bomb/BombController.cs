using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Tilemaps;


public class BombController : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombFuseTime = 3f;
    [SerializeField] private int bombAmount = 1;
    private int bombsRemaining;
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] LayerMask explosionLayerMask;
    [SerializeField] int explosionRadius = 1;
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;



    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }
    private void Update()
    {
        KeyBomb();
    }
    private void KeyBomb()
    {
        if (bombsRemaining > 0 &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(PlaceBomb());
        }
    }
    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;
        yield return new WaitForSeconds(bombFuseTime);
        if (bomb == null)
        {
            yield break;
        }
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);
        Destroy(bomb);
        bombsRemaining++;
    }
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }
    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false;
        }
    }
}
