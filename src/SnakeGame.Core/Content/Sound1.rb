use_synth :pulse
notes = [73, 76, 79, 84]
notes.each do |n|
  play n, release: 0.04, amp: 0.4
  sleep 0.02
end
