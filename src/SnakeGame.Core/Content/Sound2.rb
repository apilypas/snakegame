use_synth :pulse
notes = [66, 60, 56, 52]
a = 0.5
notes.each do |n|
  play n, release: 0.2, amp: a
  a = a - 0.1
  sleep 0.06
end
