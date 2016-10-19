#pragma strict

function Start () {

    var lineStart : Vector2 = new Vector2(1,1);
    var lineEnd : Vector2 = new Vector2(1,10);

    var fullDirection = lineEnd-lineStart;
    var lineDirection = Normalize(fullDirection);
    //var closestPoint = Vector2.Dot((point-lineStart),lineDirection)/Vector2.Dot(lineDirection,lineDirection);

}


static function Normalize (p : Vector2) {
	var mag : float = p.magnitude;
	return p/mag;
}