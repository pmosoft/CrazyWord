#pragma strict

var rotating = false; // 회전 boolean 

var Target: Transform; // 회전할 타켓 현재는 큐브 

var rotateTime = 3.0; // 회전시간 

var rotateDegrees = 90.0; // 회전할 각도량 

var rotateVector  = Vector3.zero; // 각 벡터 

var keyCode : KeyCode = KeyCode.Space; // 제어 버튼 선택리스트 

function Start() 
{ 
  if(Target == null) // 타켓이 없으면 큐브를 찾아서 할당 
  { 
      Target = GameObject.Find("Cube").transform; 
  }  

  rotateVector = Vector3(0, rotateDegrees, 0); 

} 
function Update() 
{ 
	// 아래의 키를 가지고 큐브를 회전 
	if (Input.GetKeyDown(keyCode)) 
	{ 
	    RotateObject(Target, rotateVector, rotateTime); 
	} 

} 

function RotateObject (thisTransform : Transform, degrees : Vector3, seconds : float) { 
    if (rotating) return; 
    rotating = true; 

    var startRotation = thisTransform.rotation; // 시작 로테이션 할당 
    var endRotation = thisTransform.rotation * Quaternion.Euler(degrees); // 끝 로테이션 할당 
    var t = 0.0; 
    var rate = 1.0/seconds; 

    while (t < 1.0) { 
        t += Time.deltaTime * rate; 
        thisTransform.rotation = Quaternion.Slerp(startRotation, endRotation, Mathf.SmoothStep(0.0, 1.0, t)); 
        yield; 
    } 

    rotating = false; 
}