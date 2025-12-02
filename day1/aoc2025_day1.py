import sys
from pathlib import Path

def count_end(instructions):
    d=50;z=0
    for ins in instructions:
        if not ins: continue
        dir=ins[0].upper();dist=int(ins[1:])
        if dir=='L': d=(d-dist)%100
        else: d=(d+dist)%100
        if d==0: z+=1
    return z

def count_method(instructions):
    d=50;z=0
    for ins in instructions:
        if not ins: continue
        dir=ins[0].upper();dist=int(ins[1:])
        if dir=='R':
            s=d
            t0=(100-s)%100
            if t0==0: t0=100
            if t0<=dist: z+= (dist-t0)//100 +1
            d=(d+dist)%100
        else:
            s=d
            t0=s%100
            if t0==0: t0=100
            if t0<=dist: z+= (dist-t0)//100 +1
            d=(d-dist)%100
    return z

def main():
    possible=[Path(__file__).parent/"data_points_aoc2025.txt", Path(__file__).parent/"data_points.txt", Path("data_points_aoc2025.txt"), Path("data_points.txt")]
    if len(sys.argv)>=2: possible.insert(0, Path(sys.argv[1]))
    f=None
    for p in possible:
        if p.exists(): f=p; break
    if f is None:
        print("File not found!"); sys.exit(1)
    ins=[l.strip() for l in f.read_text().splitlines() if l.strip()]
    print(count_end(ins))
    print(count_method(ins))

if __name__=="__main__": main()