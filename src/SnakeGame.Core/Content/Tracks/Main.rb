use_bpm 80
use_random_seed 42069
b = true

live_loop :bassline do
  if b
    use_synth :chipbass
    play :c2, release: 0.25, pan: -0.3
    sleep 0.5
    play :e2, release: 0.25, pan: 0.3
    sleep 0.5
    play :g2, release: 0.25, pan: -0.3
    sleep 0.5
    play :b1, release: 0.25, pan: 0.3
    sleep 0.5
  else
    sleep 1
  end
end

sleep 4
live_loop :random_lead, sync: :bassline do
  use_synth :chiplead
  26.times do
    n = (scale :c4, :major_pentatonic).choose
    play n, release: 0.2, amp: 0.8, pan: [0.3, 0.1, 0, -0.1, -0.3].choose
    sleep 0.25
  end
  sleep 8
  b = true
end

sleep 10
live_loop :drums, sync: :bassline do
  use_synth :chiplead
  16.times do
    4.times do
      play :e4, release: 0.2, amp: 0.6
      sleep 0.25
    end
    4.times do
      play :f4, release: 0.2, amp: 0.6
      sleep 0.25
    end
  end
  sleep 4
  b = false
end
