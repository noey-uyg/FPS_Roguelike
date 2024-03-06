# 1인칭 FPS 로그라이크 게임
프로젝트 기간 : 24.01.15 ~ 24.03.06
프로젝트 인원 : 위규연

# 참고
유튜버 케이디님의 “[유니티 3D 강좌] FPS 서바이벌 디펜스”  

https://youtu.be/fxaDBE71UHA?si=2t_p2WUbPb_OSrNr  

https://youtu.be/waEsGu--9P8?si=N5ZJ2J9DG0B7WW6J  

https://youtu.be/2ajD1GDbEzA?si=6s5_AzwgS0cY_FeU  

# 설명
- 1인칭 FPS 로그라이크 게임입니다.
- 레벨업 시 나타나는 여러 각성을 통해 캐릭터를 강화하여 보스를 처치해야 합니다.
- 4가지의 난이도가 있고, 난이도 별로 적의 공격력과 체력이 조정됩니다.
- 캐릭터의 특성과 퍼즐을 이용하여 캐릭터를 강화할 수 있습니다.
- 캐릭터의 특성은 게임 중 적을 처치하여 얻은 다이아를 통해 강화할 수 있습니다.
- 퍼즐은 최종 보스를 처치하여 얻을 수 있고, 5x5의 인벤토리에 배치하여 능력을 적용할 수 있습니다.
- 웨이브를 시작하기 전 충분한 준비 시간을 가진 후 F를 길게 눌러 웨이브를 시작할 수 있습니다.
- 웨이브를 시작한 후에 상점이 나타나지만 1회 이용 시 사라지게 됩니다.
<details markdown="1">
  <summary><b>웨이브</b></summary>
  <ul>
    <li>1웨이브 - 1000마리 잡으면 끝(마지막에 엘리트 몬스터 3마리 잡기)</li>
    <li>2웨이브 - 3000마리 잡으면 끝(강화된 몬스터가 나오기 시작함, 마지막에 엘리트 3마리 잡기)</li>
    <li>3웨이브 - 5000마리 잡으면 끝(강화된 몬스터가 증가하여 나오기 시작함, 마지막에 엘리트 3마리 잡기)</li>
    <li>4웨이브 - 보스웨이브</li>
  </ul>
</details>  
<details markdown="1">
  <summary><b>상점</b></summary>
  <ul>
    <li>상점에서 얻을 수 있는 아이템입니다.</li>
    <li>적 처치 시 얻는 골드로 구매가능합니다.</li>
    <li>웨이브가 지날수록 비싸집니다.</li>
    <li>힐 아이템</li>
    <li>랜덤 스크롤</li>
    <li>랜덤 각성</li>
    <li>무기 강화 (새로고침 당 최대 2번)</li>
  </ul>
</details>  
<details markdown="1">
  <summary><b>드랍 아이템</b></summary>
  <ul>
    <li>적 처치 시 얻을 수 있는 아이템입니다.</li>
    <li>총 장착 시 총알 드랍(100%로 1~3개 1개당 30발)</li>
    <li>캐릭터 특성을 찍을 수 있는 다이아(0~3개 랜덤 드랍)</li>
    <li>골드(0~5원 랜덤 드랍))</li>
    <li>힐 아이템(5%로 드랍)</li>
    <li>스크롤(0.5%로 1개 드랍)</li>
  </ul>
</details>  

# 플레이어
<details markdown="1">
  <summary></summary>
  <ul>
    <li>기본 체력 : 120</li>
    <li>기본 이동속도 : 8</li>
    <li>달리기 속도 : 20</li>
    <li>앉은 상태 속도 : 3</li>
    <li>레벨업에 필요한 경험치 : [12, 19, 28, 42, 56, 63, 70, 81, 93, 109, 121, 136, 155, 166, 189, 190, 200, 201, 213, 217, 220, 228, 231, 233, 235, 236, 245, 251, 274, 288, 297, 324, 356, 378, 403, 456, 484, 499, 518, 553]</li>
    <li>기본 치명타 데미지는 무기 기본 데미지의 150%입니다.</li>
  </ul>
</details>

# 무기
<details markdown="1">
  <summary></summary>
  <ul>
    <h3>1. 주먹</h3>
    <li>공격력 : 7</li>
    <li>사정거리 : 10</li>
    <li>치명타 확률 : 5%</li>
  </ul>
  <ul>
    <h3>2. 총</h3>
    <li>공격력 : 10</li>
    <li>사정거리 : 100</li>
    <li>치명타 확률 : 15%</li>
  </ul>
  <ul>
    <h3>3. 도끼</h3>
    <li>공격력 : 20</li>
    <li>사정거리 : 15</li>
    <li>치명타 확률 : 10%</li>
  </ul>
</details>

# 적
<details markdown="1">
  <summary></summary>
  <ul>
    <h3>기본 몬스터</h3>
    <li>최소 체력 : 50</li>
    <li>최대 체력 : 130</li>
    <li>최소 공격력 : 5</li>
    <li>최대 공격력 : 12</li>
    <h3>강화 몬스터</h3>
    <li>체력 : 기본 몬스터의 5배</li>
    <li>최소 공격력 : 5</li>
    <li>최대 공격력 : 12</li>
    <h3>엘리트 몬스터</h3>
    <li>체력 : 기본 몬스터의 10배</li>
    <li>공격력 : 15</li>
    <h3>보스 몬스터</h3>
    <li>체력 : 기본 몬스터의 100배</li>
    <li>공격력 : 12</li>
    <li>폭발 공격력 : 10</li>
    <h3>ETC</h3>
    <li>적의 체력은 웨이브와 현재 레벨에 비례하여 증가합니다.</li>
    <li>난이도 별 체력과 공격력 Easy : 75%, Normarl : 100%, Hard : 150%, Nightmare = 300%</li>
  </ul>
</details>

# 각성
<details markdown="1">
  <summary></summary>
  <ul>
    <h3>주먹 전용</h3>
    <li>주먹으로 공격 시 파동을 생성해서 피격당한 적의 뒤에 있는 적들에게도 200%/350%/500% 데미지를 입힌다.</li>
    <li>주먹으로 적 공격시 스택이 쌓이게 되고 스택당 10%/20%/40%의 데미지가 강력해진다. (최대 8스택) </li>
    <li>주먹의 기초 데미지가 증가한다 100%/300%/600%</li>
    <h3>총 전용</h3>
    <li>총으로 적을 피격 시 연쇄를 일으키는 번개를 생성하여 1명/3명/5명의 적을 무기 데미지로 공격한다.</li>
    <li>총의 기초 데미지가 증가한다 100%/150%/250%</li>
    <li>총으로 사격이 계속되면 사격속도가 증가한다. 10%/15%/20% (최대3스택)</li>
    <h3>도끼 전용</h3>
    <li>도끼로 적을 공격시 1초마다 현재 데미지의 15%/25%/40%의 데미지를 입힌다.</li>
    <li>도끼를 사용할 시 적들이 공포에 시달려 받는 피해량이 증가한다. 5%/10%/20%</li>
    <li>도끼의 기초 데미지가 증가한다 100%/250%/450%</li>
    <h3>공용</h3>
    <li>사정거리 증가 10%/20%/40%</li>
    <li>이동속도 증가 10%/25%/50%</li>
    <li>치명타 확률 증가 20%/25%/35%</li>
    <li>치명타 데미지 증가 20%/40%/60%</li>
    <li>최종 데미지 증가 15%/25%/40%</li>
    <li>최대 생명력 증가 20%/30%/50%</li>
  </ul>
</details>

# 특성
[n] = 비용
<details markdown="1">
  <summary></summary>
  <ul>
    <h3>자원 관련</h3>
    <li>[10]초기 자금 100~500(Lv5)</li>
    <li>[10]상점에서 랜덤 각성을 얻을 수 있게 됨(Lv1)</li>
    <li>[30]금화 획득시 10~50%확률로 2배 획득(Lv5)</li>
    <li>[90]상점 새로고침 가능(Lv1)</li>
    <li>[120]엘리트, 보스 몬스터 처치 시 스크롤 획득확률 증가 (Lv1)</li>
    <h3>데미지 관련</h3>
    <li>[10] 데미지(Lv5)</li>
    <li>[10]치명타 확률 증가</li>
    <li>[30] 총의 공격력 증가(Lv5)</li>
    <li>[30] 도끼의 공격속도 증가(Lv5)</li>
    <li>[30] 주먹의 사정거리 증가(Lv5)</li>
    <li>[90] 엘리트, 보스 몬스터 추가 데미지(Lv5)</li>
    <h3>게임 관련</h3>
    <li>[10] 최대 체력(Lv5)</li>
    <li>[10] 부활 가능(Lv1)</li>
    <li>[60] 받는 데미지 감소(Lv5)</li>
  </ul>
</details>

# 스크롤
<details markdown="1">
  <summary></summary>
  <ul>
    <li>치명타가 발동되지 않을 시 데미지 +30%</li>
    <li>생명력이 가득 차있을 때 데미지 +30%</li>
    <li>데미지를 받은 후 5초간 최대 생명력의 3% 회복</li>
    <li>생명력 70% 이상의 적을 공격시 데미지 +30%</li>
    <li>무기 변환 시 5초간 데미지 +150%</li>
    <li>부활 횟수 +1</li>
    <li>공격시 5% 확률로 최대 체력의 10%의 데미지를 입힘</li>
    <li>최대 생명력이 50% 증가하고 받는 피해량이 50% 증가한다.</li>
    <li>엘리트 몬스터 처치 시마다 데미지+10%</li>
    <li>모든 각성을 선택할 수 있다.(얻은 후 무조건 선택해야함, 선택 후 다시 사용 X)</li>
    <li>최대 생명력-50%, 데미지 +50%</li>
    <li>적을 처치할 때마다 데미지 +1%</li>
    <li>상점의 새로고침 횟수 +1</li>
  </ul>
</details>

# 퍼즐
<details markdown="1">
  <summary></summary>
  <ul>
    <h3>1x1</h3>
    <li>적 50마리 처치 시마다 데미지+1%</li>
    <li>적 50마리 처치 시마다 최대 생명력+1%</li>
    <li>적 50마리 처치 시마다 이동속도+0.1%</li>
    <h3>1x2</h3>
    <li>레벨업 시 데미지 +1.5%</li>
    <li>레벨업 시 생명력 회복</li>
    <li>레벨업 시 이동속도 +0.2%</li>
    <h3>2x2</h3>
    <li>맨손 기본 데미지 +50%</li>
    <li>총 기본 데미지 +50%</li>
    <li>도끼 기본 데미지 +50%</li>
    <h3>1x4</h3>
    <li>스크롤 소유 개수마다 데미지 +2%</li>
    <li>치명타 시 적이 5초동안 받는 데미지 +25%</li>
    <h3>2x5</h3>
    <li>부활횟수 +1</li>
    <li>부활 후 데미지, 이동속도, 최대 생명력 +10% 증가</li>
    <h3>3x3</h3>
    <li>적 처치시 주변의 적에게 최대 생명력의 30%의 데미지를 준다.</li>
    <li>치명타 시 주변의 적에게 최대 생명력의 5%의 데미지를 준다.</li>
  </ul>
</details>